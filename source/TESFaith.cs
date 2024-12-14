using System;
using System.IO;
using ArrayList=System.Collections.ArrayList;
using MessageBox=System.Windows.Forms.MessageBox;
using ToolStripProgressBar=System.Windows.Forms.ToolStripProgressBar;

namespace TESFaith {
    public class TESFaith {
        private static CellData Cell=new CellData();
        private static StreamWriter writer;

        private static ArrayList CellChangedList=new ArrayList();
        
        public static ArrayList Rules=new ArrayList();
        public static ArrayList CellList=new ArrayList();

        private static bool verbose_mode=false;	         /* Command line option: Option to produce an extensive log file.             */
        private static bool list_cell_mode=false;	     /* Command line option: Option to produce a file listing the plugin's cells. */
        private static bool modify_script_mode=true;     /* Command line option: Option to modify scripts if necessary: Default on.   */
        private static bool generate_specific_esp=false; /* Command line option: Option to create a new ESP only containing changes.  */
        private static bool generate_log=true;
        private static bool internal_list_mode=false;

        public static void SetOptions(bool VerboseMode,bool ListCells,bool ModifyScript,bool NewESP,bool Log,
            bool InternalList) {
            verbose_mode=VerboseMode;
            list_cell_mode=ListCells;
            modify_script_mode=ModifyScript;
            generate_specific_esp=NewESP;
            generate_log=Log;
            internal_list_mode=InternalList;
        }

        private static int pass=0;		                /* Current file pass. Tesfaith runs through the ESP/ESM in 2 passes.         */
        private static int total_cells=0;		        /* Total CELL records found in the file.*/
        private static int total_land=0;		        /* Total LAND records found in the file.*/
        private static int total_records_changed=0;    /* LAND or CELL records changed/copied.*/
        private static int total_objects=0;		    /* Total objects found in the file.*/
        private static int total_objects_changed=0;    /* Total objects changed/copied.*/
        private static int total_scripts=0;		    /* Total SCPT records found in the file. */
        private static int total_scripts_changed=0;	/* Total SCPT records modified.*/
        private static int total_dialogs=0;            /* Total INFO scripts found in the file. */
        private static int total_dialogs_changed=0;	/* Total INFO records modified.*/

        public static void RunRules(string Path, ref ToolStripProgressBar ProgressBar) {
            CellChangedList.Clear();
            CellList.Clear();

            int size;	/* Size of current record.             */
            int new_size;   /* Size of modified version of record. */
            string RecordType;

            ByteStream header=null;	/* For storing the 16-byte header. */
            ByteStream or=null;	/* Pointer to the Original Record. */
            ByteStream nr=null;	/* Pointer to the New Record.      */

            //Open a few files
            FileStream InputMod,OutputMod=null;
            string InFile=null;
            if(!list_cell_mode) {
                for(int i=0;i<999;i++) {
                    InFile=Path+"."+i.ToString().PadLeft(3,'0');
                    if(!File.Exists(InFile)) break;
                }
                File.Copy(Path,InFile);
                InputMod=new FileStream(InFile,FileMode.Open,FileAccess.Read);
                OutputMod=new FileStream(Path,FileMode.Create,FileAccess.Write);
                if(generate_log) writer=new StreamWriter("TESFaith# Plus - log.txt");
            } else {
                InputMod=new FileStream(Path,FileMode.Open,FileAccess.Read);
            }
            // Initialize variables:
            Cell.name="";
            Cell.region_name="";
            Cell.current_x=0;
            Cell.current_y=0;
            Cell.new_x=0;
            Cell.new_y=0;
            Cell.size=0;
            Cell.type=CellType.Interior;
            Cell.save=true;
            Cell.copy=false;

            if(!modify_script_mode)
                Log("Scripts and Dialog Commands will not be modified.\n");

            var InputModSize=InputMod.Length;
            try {
                for(pass=1;pass<=2;pass++) {
                    InputMod.Position=0;
                    if(OutputMod!=null) OutputMod.Position=0;

                    if(pass<2) {
                        Log("Step 1/2: Generating a list of CELLs that should be modifed\n");
                    } else if(list_cell_mode) {
                        Log("\nStep 2/2: Writing out the list of CELLs\n");
                    } else {
                        Log("\nStep 2/2: Processing data\n");
                    }

                    // TES records follows the format of TYPE (4 bytes) then
                    // the record length (4 bytes) - which we put in to the variable, size.
                    while(true) {
                        try {
                            header=new ByteStream(InputMod,16);
                        } catch { break; }

                        Cell.size=0;
                        Cell.current_x=0;
                        Cell.current_y=0;
                        Cell.new_x=0;
                        Cell.new_y=0;
                        Cell.type=CellType.Interior;
                        Cell.save=true;
                        Cell.copy=false;

                        RecordType=header.GetString(0,4);
                        size=header.GetInt(4);
                        Cell.size=size;
                        new_size=size;

                        /* Create some memory space to store the full orignal record
                         * and an additional copy (nr) which may be modified.
                         * A cheap hack here adds 2048 bytes to the memory allocated
                         * to the new record. This is to cope with a compiler problem
                         * (with Cygwin when compiling a static binary) with reallocing
                         * more space (causes problems with free(or)) later.
                         * A Morrowind script can now grow by 2K without causing any
                         * problems - e.g. That's several hundred position updates in one script.
                         */

                        byte[] record=new byte[size+16];
                        byte[] newrecord=new byte[size+16];
                        InputMod.Position-=16;
                        ProgressBar.Value = (int)(((double)InputMod.Position / (double)InputModSize) * 100.0d);
                        InputMod.Read(record,0,size+16);
                        record.CopyTo(newrecord,0);
                        or=new ByteStream(record);
                        nr=new ByteStream(newrecord);

                        if(verbose_mode&&pass==2) Log("New Record: "+RecordType+": ("+size.ToString()+" bytes)");
                        /******************************************************
                         ** If it's a CELL or a LAND record, then hand it on to
                         ** a procedure that will handle the format, including
                         ** determining if any modifications should be made.
                         *****************************************************/
                        if(RecordType=="CELL") {
                            Cell.name="";
                            Cell.region_name="";
                            total_cells++;
                            nr.AddOffset(16);
                            ProcessCellData(nr,size);
                            nr.RemoveOffset();
                        } else if(RecordType=="LAND") {
                            total_land++;
                            if(pass==2) {
                                nr.AddOffset(16);
                                ProcessLANDData(nr,size);
                                nr.RemoveOffset();
                            }
                        } else if(RecordType=="SCPT") {
                            if(pass==2) {
                                total_scripts++;
                                if(modify_script_mode) ProcessSCPTData(nr,ref new_size);
                                else if(verbose_mode) Log("\n");
                            }
                        } else if(RecordType=="INFO") {
                            if(pass==2) {
                                total_dialogs++;
                                if(modify_script_mode) ProcessINFOData(nr,ref new_size);
                                else if(verbose_mode) Log("\n");
                            }
                        } else {
                            if(verbose_mode&&pass==2) Log(" - ignoring record (not CELL, LAND, SCPT or INFO)\n");
                        }

                        // If this record is marked as save, then save the original
                        // unmodified record (*or) to the new file.
                        // If generate_specific_esp is set, only save the TES3 record.

                        if(pass==2&&!list_cell_mode&&Cell.save==true
                            &&(!generate_specific_esp||RecordType=="TES3")) {

                            // If we're generating a fresh ESP of just the changes, make sure
                            // that byte 0x1C (28) is set to 0. ESM files have it set to 1.
                            if(generate_specific_esp&&RecordType=="TES3") or.WriteByte(28,0);

                            if(verbose_mode) Log("\t[Saving Original Record: "+size.ToString()+" bytes]\n");

                            OutputMod.Write(or.GetBytes(0,size+16),0,size+16);
                        }

                        /**********************************************************
                         * If this record is marked as copy, then save the new
                         * modified record (*nr) to the new file.
                         *********************************************************/

                        if(pass==2&&!list_cell_mode&&Cell.copy==true&&
                            (!generate_specific_esp||RecordType=="CELL"||RecordType=="LAND")) {

                            if(verbose_mode) Log("\t[Saving New (modified) Record: "+new_size.ToString()+" bytes]\n");
                            OutputMod.Write(nr.GetBytes(0,new_size+16),0,new_size+16);
                        }
                        if(or!=null) { or.Close(); or=null; }
                        if(nr!=null) { nr.Close(); nr=null; }
                    }

                    if(list_cell_mode) {
                        pass=2;
                        break;
                    }
                }

                if(InputMod!=null) InputMod.Close();
                if(OutputMod!=null) OutputMod.Close();

            } catch {
                MessageBox.Show("An Error occurered. The plugin was not modified","Oops!");
                try {
                    if(InputMod!=null) InputMod.Close();
                    if(OutputMod!=null) OutputMod.Close();
                } catch { }
                File.Delete(Path);
                    File.Move(InFile,Path);
            }

            // Dump out the running totals.

            if(!list_cell_mode) {
                string FinalMessage=
                    "Total CELL records found:\t\t\t\t"+total_cells.ToString()+"\n"+
                    "Total LAND records found:\t\t\t\t"+total_land.ToString()+"\n"+
                    "Total CELL/LAND records modified or copied:\t\t"+total_records_changed.ToString()+"\n"+
                    "Total objects modified/found:\t\t\t"+total_objects_changed.ToString()+
                        "/"+total_objects.ToString()+"\n"+
                    "Total Script Commands modified/Scripts found:\t"+total_scripts_changed.ToString()+
                        "/"+total_scripts.ToString()+"\n"+
                    "Total Dialog entries modified/found:\t\t\t"+total_dialogs_changed.ToString()+
                        "/"+total_dialogs.ToString()+"\n";
                MessageBox.Show(FinalMessage,"Finished!");
                Log(FinalMessage);
            }

            if(generate_log) writer.Close();
        }

        /// <summary>
        /// Processes a CELL record, extracting name, region and any
        ///	FRMR (objects in this cell) records (if they exist).
        /// </summary>
        /// <remarks>
        /// Format:
        /// CELL (4) + Length (12)
        /// NAME (4) + Length (4) + NameAsString (size given by Length)
        /// DATA (4) + Unknown (16).
        /// RGNN (4) + Length (4) + RegionAsString (size given by Length)
        /// </remarks>
        /// <param name="data">The contents of the CELL record</param>
        private static void ProcessCellData(ByteStream data,int size) {

            int nsize;
            int pos=0;
            int xypos=0;
            /**********************************
             * NAME of the cell (8+Name bytes).
             *********************************/
            nsize=data.GetInt(4);
            Cell.name=data.GetString(8,nsize-1);
            pos=8+nsize;
            /* The DATA section (4+16 = 20 bytes)
             * Bytes 12-19 contain two integers that represent the (x, y)
             * co-ordinates of this cell in the world grid.
             * The variable xypos is a useful reference for this position,
             * should we need to change these values later.*/
            xypos=pos+12;
            Cell.current_x=data.GetInt(xypos);
            Cell.current_y=data.GetInt(xypos+4);
            Cell.new_x=Cell.current_x;
            Cell.new_y=Cell.current_y;

            pos+=20;
            /**********************************
             * RGNN Region data (8+Name bytes).
             *********************************/
            if(Cell.name==""||data.GetString(pos,4)=="RGNN") {
                Cell.type=CellType.Exterior;
                if(data.GetString(pos,4)=="RGNN") {
                    nsize=data.GetInt(pos+4);
                    Cell.region_name=data.GetString(pos+8);
                    pos+=8+nsize;
                }
                if(list_cell_mode) {
                    LogCell(Cell.type);
                    return;
                }
                CheckRules(null);
                //Write the cell to the log file if in verbose mode, or if it has been moved
                if(pass==2&&(verbose_mode||Cell.current_x!=Cell.new_x||Cell.current_y!=Cell.new_y)) {
                    Log(" [EXTERIOR] ");
                    if(Cell.name=="") {
                        Log("(CELL has no name) ");
                    } else {
                        Log(" \""+Cell.name+"\" ");
                    }
                    if(Cell.region_name=="") {
                        Log("(no associated REGION) ");
                    } else {
                        Log(" \""+Cell.region_name+"\" ");
                    }
                    Log(" ("+Cell.current_x.ToString()+", "+Cell.current_y.ToString()+") ");
                    if(verbose_mode&&Cell.current_x==Cell.new_x&&Cell.current_y==Cell.new_y) Log("\n");
                }
                if(Cell.current_x!=Cell.new_x||Cell.current_y!=Cell.new_y) {
                    // If a cell is being moved, make a note of the cell co-ordinates.
                    // This is the entire purpose of the first pass. The list of cells
                    // to be changed can then be backreferenced by other records, such
                    // as Interior->Exterior Teleport doors, scripts, NPCs and Dialogue commands.
                    if(pass==1) {
                        if(!Cell.save)
                            WriteCellChangeList(Cell.current_x,Cell.current_y,
                                Cell.new_x-Cell.current_x,Cell.new_y-Cell.current_y);
                        return;
                    } else {
                        data.WriteInt(xypos,Cell.new_x);
                        data.WriteInt(xypos+4,Cell.new_y);
                        total_records_changed++;
                        Log("- MODIFIED to ("+Cell.new_x.ToString()+", "+Cell.new_y.ToString()+").\n");
                    }
                }
            } else {   /* INTERIOR cell.  */
                Cell.type=CellType.Interior;
				if (list_cell_mode)
				{
					LogCell(Cell.type);
					return;
				}
				CheckRules(data);
                if(verbose_mode&&pass==2) Log(" [INTERIOR] \""+Cell.name+"\" \n");
            }

            if(pos>=size||list_cell_mode) return;

            // Skip NAMn (4 bytes for marker, 4 bytes for Length),
            // We also skip the length itself.
            if(data.GetString(pos,3)=="NAM") {
                nsize=data.GetInt(pos+4);
                pos+=8+nsize;
            }

            // FRMR record.
            while(pos<size) {
                if(data.GetString(pos,4)=="FRMR"&&pass==2) {
                    total_objects++;
                    data.AddOffset(pos);
                    pos+=ProcessFRMRData(data,size-pos);
                    data.RemoveOffset();
                } else {
                    pos+=4;
                }
            }
        }

        /// <summary>
        /// Processes a FRMR subrecord: Each object stored in a CELL record is a FRMR subrecord.
        /// </summary>
        /// <remarks>
        /// FRMR (4 bytes) + Length (4 bytes) + Index (4 bytes)
        /// NAME (4 bytes) + Length (4 bytes) + NameData (length bytes) +
        ///                + Scale (4) + Scale Length (4) + ScaleData (4)
        /// FLTV: LockInfo (optional 12 bytes).
        /// KNAM: KeyInfo (optional 4 KNAM + 4 length + length bytes name).
        /// TNAM: Trap Info (optional 4 TNAM + 4 length + length bytes name).
        /// DATA (4 bytes) + Length (4 bytes) + XYZ Data (24 bytes).
        /// </remarks>
        /// <param name="data">The contents of the CELL record offset to the start of this record</param>
        /// <param name="record_size">The size of the CELL record minus the offset</param>
        /// <returns>The number of bytes taken up by the FRMR record</returns>
        private static int ProcessFRMRData(ByteStream data,int record_size) {
            string name;
            int pos=12; // Skip the FRMR and index data (12 bytes) and NAME marker (4).
            int nsize;
            float xpos,ypos;

            if(data.GetString(pos,4)=="NAME") {
                Log("Unknown field in FRMR entry: "+data.GetString(pos,4)+"\n");
                pos=record_size;
                return pos;
            }
            pos+=4;

            nsize=data.GetInt(pos);
            name=data.GetString(pos+4,nsize);
            pos+=4+nsize;

            if(verbose_mode) Log("\tObject: \""+name+"\"\t");
            /************************************************************
            * Many optional object fields may now follow. We're
            * only looking for the DATA field (contains the
            * (xyz co-ords of the object). All other fields are
            * skipped. Some objects, strangely (e.g. a couple in
            * "Siege at Firemoth"), don't contain co-ordinates; if FRMR
            * is encountered then this function returns, so the calling
            * function can process the next object.
            * Any unknown fields are spat out as warnings and ignored.
	        ***********************************************************/
            while(data.GetString(pos,4)!="DATA") {
                if(pos>=record_size-1) return pos;
                string FieldName=data.GetString(pos,4);
                if(FieldName=="FRMR") {
                    return pos;
                } else if(FieldName=="DODT"&&Cell.type==CellType.Interior) {
                    data.AddOffset(pos);
                    ProcessDODTData(data,record_size-pos);
                    data.RemoveOffset();
                } else if(verbose_mode&&FieldName!="FLTV"&&FieldName!="KNAM"&&FieldName!="XSCL"&&
                          FieldName!="DNAM"&&FieldName!="ANAM"&&FieldName!="INTV"&&FieldName!="NAM9"&&
                          FieldName!="INDX"&&FieldName!="TNAM"&&FieldName!="DELE"&&FieldName!="XCHG"&&
                          FieldName!="CNAM") {
                    Log("[Warning: <Unknown FRMR field: "+FieldName+">]!\n");
                }
                nsize=data.GetInt(pos+4);
                pos+=8+nsize;
            }
            /* What we really wanted all along; the DATA field.
            *  DATA (4 bytes), length (4bytes), X, Y, Z (4 bytes each) and
            *  rotX, rotY, rotZ (4 bytes each) - all 32-bit floats.         */
            pos+=8;

            xpos=data.GetFloat(pos);
            ypos=data.GetFloat(pos+4);

            if(Cell.current_x!=Cell.new_x||Cell.current_y!=Cell.new_y) {
                Log("\tObject \""+name+"\"\t("+xpos.ToString()+", "+ypos.ToString()+") ");
                xpos+=(float)(8192*(Cell.new_x-Cell.current_x));
                data.WriteFloat(pos,xpos);
                ypos+=(float)(8192*(Cell.new_y-Cell.current_y));
                data.WriteFloat(pos+4,ypos);
                Log(" - MODIFIED to ("+xpos.ToString()+", "+ypos.ToString()+")\n");
                total_objects_changed++;
            } else if(verbose_mode) Log("("+xpos.ToString()+", "+ypos.ToString()+")\n");

            pos+=24;
            return pos;
        }

        /// <summary>
        /// Process a DODT teleporting door record.
        /// </summary>
        /// <remarks>
        /// First do an entire scan for a DNAM field - this only exists for
        /// Doors pointing to Interior locations, which we may ignore.
        /// Then look for the DODT field which contains the XYZ teleport
        /// co-ordinates - these values are checked against the temp cell change
        /// list file and if it points to a location that is to be moved, the
        /// door's destination X and Y co-ordinates are modified accordingly.
        ///
        /// DODT (4 bytes) + Length (4 bytes) + XYZ Data (24 bytes).
        /// </remarks>
        /// <param name="data">The CELL record, offset to the start of this DODT record</param>
        /// <param name="record_size">the size of the CELL record minus the offset</param>
        /// <returns>The number of bytes taken up by the DODT record</returns>
        private static int ProcessDODTData(ByteStream data,int record_size) {
            int pos=0;
            int dodt_pos=0;
            int nsize;
            int int_x;    		// World Cell (X,Y) that door teleports to.
            int int_y;
            int trans_x=0;
            int trans_y=0;
            float xpos,ypos;

            if(verbose_mode) Log("\tInterior Door: ");

            /************************************************************
            * Many optional object fields may now follow. We scan for
            * the DATA field or end of record, but our primary concern
            * is whether a DNAM field exists. If it does, then it's an
            * Interior->Interior cell and we don't need to worry about it.
            * Any unknown fields are spat out as warnings and ignored.
            ***********************************************************/
            while(data.GetString(pos,4)!="DATA"&&pos<record_size-1) {
                if(data.GetString(pos,4)=="DNAM") {
                    if(verbose_mode) Log("[Interior->Interior Door Teleport - ignoring.] ");
                    return pos;
                } else if(verbose_mode&&data.GetString(pos,4)!="CNAM"&&data.GetString(pos,4)=="DODT") {
                    Log("[Warning: <Unknown DODT field: "+data.GetString(pos,4)+">!\n");
                } else {
                    nsize=data.GetInt(pos+4);
                    pos+=8+nsize;
                }
            }
            /* Now we know it's not an Interior->Interior door, move the pointer back to
             * where the DODT field started and extract the destination co-ordinates.
             * DODT (4 bytes), length (4bytes), teleport X, Y, Z (4 bytes each) and
             * rotX, rotY, rotZ (4 bytes each) - all 32-bit floats.*/
            pos=dodt_pos;

            if(data.GetString(pos,4)=="DODT") {
                pos+=8;

                xpos=data.GetFloat(pos);
                ypos=data.GetFloat(pos+4);

                int_x=(int)(xpos/8192);
                int_y=(int)(ypos/8192);

                /* Negative co-ordinates need to be decremented by 1.
                * e.g. (-6000, -12000) exists in cell (-1, -2) not (-0, -1) !*/
                if(xpos<0) int_x-=1;
                if(ypos<0) int_y-=1;

                if(verbose_mode) Log("Door teleports to ("+xpos.ToString()+", "+ypos.ToString()+"): "+
                    "CELL ["+int_x.ToString()+", "+int_y.ToString()+"] ");

                if(CheckCellChangeList(int_x,int_y,out trans_x,out trans_y)) {
                    if(!verbose_mode)
                        Log("\tDoor teleport\t("+xpos.ToString()+", "+ypos.ToString()+") ");
                    xpos+=(float)(8192*(trans_x));
                    data.WriteFloat(pos,xpos);
                    ypos+=(float)(8192*(trans_y));
                    data.WriteFloat(pos+4,ypos);
                    Cell.save=false;
                    Cell.copy=true;
                    Log(" - MODIFIED to ("+xpos.ToString()+", "+ypos.ToString()+")\n");
                    total_objects_changed++;
                } else if(verbose_mode) Log("\n");
                pos+=24;
            }

            return pos;
        }

        /// <summary>
        /// Process a LAND record.
        /// </summary>
        /// <remarks>
        /// LAND (4 bytes) + Length (4 bytes) + X (4 bytes) + Y (4 bytes).
        /// </remarks>
        /// <param name="data">The record data</param>
        /// <param name="size">The record size</param>
        /// <returns>0 if success, non zero on error</returns>
        private static bool ProcessLANDData(ByteStream data,int size) {
            /*********************************************************
             * Get the (hopefully) INTV  header and X, Y co-ordinates.
	         ********************************************************/
            if(data.GetString(0,4)=="INTV") {
                Cell.current_x=data.GetInt(8);
                Cell.current_y=data.GetInt(12);
                Cell.new_x=Cell.current_x;
                Cell.new_y=Cell.current_y;
                if(verbose_mode) Log("LAND co-ords: ("+Cell.new_x.ToString()+", "+Cell.new_y.ToString()+") ");
            } else {
                Log("WARNING: Couldn't find the INTV header (got ["+data.GetString(0,4)+"] - ignoring record.\n");
                return true;
            }
            if(Cell.region_name!=""||Cell.name=="") Cell.type=CellType.Exterior;

            CheckRules(null);
            if(Cell.current_x!=Cell.new_x||Cell.current_y!=Cell.new_y) {
                data.WriteInt(8,Cell.new_x);
                data.WriteInt(12,Cell.new_y);
                total_records_changed++;
                if(!verbose_mode) Log("LAND co-ords: ("+Cell.current_x+", "+Cell.current_y+") ");
                Log(" - MODIFIED to ("+Cell.new_x.ToString()+", "+Cell.new_y.ToString()+")\n");
                Cell.region_name="";
            } else if(verbose_mode) Log("\n");

            return false;
        }

        /// <summary>
        /// Process a SCPT (script) record.
        /// </summary>
        /// <remarks>
        /// SCPT (4 bytes) + Length (12 bytes).
        /// </remarks>
        /// <param name="data">The record data</param>
        /// <param name="record_size">The size of the script (may be modified)</param>
        private static void ProcessSCPTData(ByteStream data,ref int record_size) {
            int pos=0;
            int pos_sctx=0;
            int nsize;

            int ss_record_size=0;

            string sname="";
            string line;
            string lower_line;

            MemoryStream TempStream;
            MemoryStream NewStream;
            StreamReader sr;
            StreamWriter sw;

            /*********************************************************
             * Get the (hopefully) SCTX header.
             ********************************************************/

            pos=16;

            while(data.GetString(pos,4)!="SCTX") {
                if(pos>=record_size-1) return;
                if(data.GetString(pos,4)=="SCHD") {
                    nsize=data.GetInt(pos+4);
                    sname=data.GetString(pos+8,nsize);
                    if(verbose_mode) Log(" - script called \""+sname+"\"");
                    pos+=8+nsize;
                } else {
                    if(verbose_mode&&data.GetString(pos,4)!="SCVR"&&data.GetString(pos,4)!="SCDT")
                        Log("[Warning: <Unknown SCDT field: "+data.GetString(pos,4)+">]!\n");
                    nsize=data.GetInt(pos+4);
                    pos+=8+nsize;
                }
            }

            nsize=data.GetInt(pos+4);
            pos_sctx=pos;
            pos+=8;

            TempStream=new MemoryStream(data.GetBytes(pos,nsize));
            TempStream.Position=0;
            sr=new StreamReader(TempStream);

            NewStream=new MemoryStream(nsize);
            sw=new StreamWriter(NewStream);

            while(true) {
                line=sr.ReadLine();
                if(line==null) break;
                lower_line=line.ToLower();
                if(lower_line.IndexOf("position ")!=-1||lower_line.IndexOf("placeitem ")!=-1||
                  lower_line.IndexOf("aifollow ")!=-1||lower_line.IndexOf("aiescort ")!=-1||
                  lower_line.IndexOf("aitravel ")!=-1) {
                    if(ModifyScriptLine(ref line)) {
                        if(!verbose_mode) Log(" - for script called \""+sname+"\"\n");
                        total_scripts_changed++;
                        ss_record_size+=line.Length-lower_line.Length;
                        Cell.save=false;
                        Cell.copy=true;
                    }
                }
                sw.WriteLine(line);
            }

            sr.Close();
            TempStream.Close();
            sw.Flush();

            if(Cell.copy==true) {
                record_size+=ss_record_size;

                // Size of new SCTX script text field.
                nsize+=ss_record_size;
                // Read new script back in to the memory record.
                // TODO: Fix!
                data.WriteBytes(pos_sctx+8,NewStream.GetBuffer());
                // Modify size of SCTX field.
                data.WriteInt(pos_sctx+4,nsize);
                /* Modify overall size of SCPT record. */
                nsize=data.GetInt(4)+ss_record_size;
                data.WriteInt(4,nsize);
            }

            sw.Close();
            NewStream.Close();

            if(verbose_mode) Log("\n");
        }

        /// <summary>
        /// ProcessINFOData(): Process a INFO (dialogue information) record.
        /// </summary>
        /// <remarks>
        /// INFO (4 bytes) + Length (12 bytes).
        /// </remarks>
        /// <param name="data">The record data</param>
        /// <param name="record_size">The size of the record (may be modified)</param>
        private static void ProcessINFOData(ByteStream data,ref int record_size) {
            int pos=0;
            int pos_bnam=0;
            int nsize;

            int ii_record_size=0;

            string name="";
            string line;
            string lower_line;

            MemoryStream TempStream;
            MemoryStream NewStream;
            StreamReader sr;
            StreamWriter sw;
            /*********************************************************
             * Get the (hopefully) BNAM header.
             ********************************************************/
            pos=16;

            while(data.GetString(pos,4)!="BNAM") {
                if(pos>=record_size-1) {
                    if(verbose_mode) Log("\n");
                    return;
                }
                string RecordName=data.GetString(pos,4);
                if(RecordName=="ONAM") {
                    nsize=data.GetInt(pos+4);
                    name=data.GetString(pos+8,nsize);
                    if(verbose_mode) Log(" - text for ID \""+name+"\"");
                    pos+=8+nsize;
                } else {
                    if(RecordName!="PNAM"&&RecordName!="NAME"&&RecordName!="NNAM"&&RecordName!="INAM"&&
                          RecordName!="FNAM"&&RecordName!="DNAM"&&RecordName!="ANAM"&&RecordName!="RNAM"&&
                          RecordName!="CNAM"&&RecordName!="SNAM"&&RecordName!="FLTV"&&RecordName!="SCTV"&&
                          RecordName!="SCVR"&&RecordName!="QSTF"&&RecordName!="QSTN"&&RecordName!="QSTR"&&
                          RecordName!="DELE"&&RecordName!="INTV"&&RecordName!="DATA") {
                        Log("[Warning: <Unknown INFO field: "+RecordName+">]!\n");
                    }
                    nsize=data.GetInt(pos+4);
                    pos+=8+nsize;
                }
            }

            nsize=data.GetInt(pos+4);
            pos_bnam=pos;
            pos+=8;

            TempStream=new MemoryStream(data.GetBytes(pos,nsize));
            sr=new StreamReader(TempStream);
            NewStream=new MemoryStream((int)TempStream.Length);
            sw=new StreamWriter(NewStream);

            while(TempStream.Position<TempStream.Length) {
                line=sr.ReadLine();
                lower_line=line.ToLower();
                if(lower_line.IndexOf("position ")!=-1||lower_line.IndexOf("placeitem ")!=-1||
                   lower_line.IndexOf("aifollow ")!=-1||lower_line.IndexOf("aiescort ")!=-1||
                   lower_line.IndexOf("aitravel ")!=-1) {
                    if(ModifyScriptLine(ref line)) {
                        if(!verbose_mode) Log(" - changed Dialog command text for \""+name+"\"\n");
                        total_dialogs_changed++;
                        ii_record_size+=line.Length-lower_line.Length;
                        Cell.save=false;
                        Cell.copy=true;
                    }
                }
                sw.WriteLine(line);
            }

            sr.Close();
            TempStream.Close();

            if(Cell.copy==true) {
                record_size+=ii_record_size;
                // Size of new BNAM Info Dialog Command text field.
                nsize+=ii_record_size;
                // Read new script from temp file back in to the memory record.
                data.WriteBytes(pos_bnam+8,NewStream.GetBuffer());
                // Modify size of BNAM field.
                data.WriteInt(pos_bnam+4,nsize);
                // Modify overall size of INFO record.

                nsize=data.GetInt(4)+ii_record_size;
                data.WriteInt(4,nsize);
            }

            sw.Close();
            NewStream.Close();

            if(verbose_mode) Log("\n");
        }

        /// <summary>
        /// Check whether a Script Command that uses co-ordinates needs
        /// updating to new co-ordinates and modify the original
        /// line accordingly.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>false if no change needed to be made or true if line was modified.</returns>
        private static bool ModifyScriptLine(ref string line) {
            int num=0;
            bool follow=false; // For AIFollow or AIEscort commands: 0,0 co-ords must not be processed.
            int ignorenums=0;  // For AIFollow,AIEscort and PlaceItem: Ignore the first few numbers.

            int icommars=0;
            int not_space=0;

            int int_x,int_y,trans_x,trans_y;

            float fx,fy,new_fx,new_fy;

            string[] coord=new string[3];
            string[] p=new string[4];
            int[] indexes=new int[4];
            int s;
            string lower_line;
            string new_line;

            lower_line=line.ToLower();

            if((s=lower_line.IndexOf("position "))==-1&&(s=lower_line.IndexOf("aitravel "))==-1) {
                if((s=lower_line.IndexOf("placeitem "))!=-1) {
                    ignorenums=1;
                } else if((s=lower_line.IndexOf("aifollow "))!=-1||(s=lower_line.IndexOf("aiescort "))!=-1) {
                    follow=true;
                    ignorenums=2;
                }
            }
            // If the pointer isn't equal to the beginning of the line and the preceding char is a alphanumeric,
            // then abort - this is another command (e.g. don't confuse position with moddisposition)
            if(s!=0&&char.IsLetterOrDigit(lower_line[s-1])) return false;

            for(num=0;num<2;num++) {
                if(ignorenums>0&&num==0) {
                    while(!char.IsWhiteSpace(lower_line[s])&&s!=lower_line.Length-1) {
                        s++;
                    }
                    while(!char.IsWhiteSpace(lower_line[s])&&s!=lower_line.Length-1) {
                        s++;
                    }
                } else {
                    while(!char.IsDigit(lower_line[s])&&lower_line[s]!='-'&&lower_line[s]!='.'&&lower_line[s]!='\0') {
                        s++;
                    }
                }
                if(s==lower_line.Length-1) return false;

                if(ignorenums>0&&num==0) {
                    while(ignorenums>0) {
                        icommars=0;
                        not_space=0;
                        ignorenums--;

                        while(lower_line[s]!='\0'&&((icommars>0&&icommars<2)||(lower_line[s]!=' '
                           &&lower_line[s]!=','&&not_space==0))) {
                            if(lower_line[s]=='\"') icommars++;
                            if(char.IsWhiteSpace(lower_line[s])||lower_line[s]==',') not_space++;
                            s++;
                        }
                        while(!char.IsDigit(lower_line[s])&&lower_line[s]!='-'&&lower_line[s]!='.'&&lower_line[s]!='\0') {
                            s++;
                        }
                    }
                }
                // Record exactly where this number begins.
                p[2*num]=lower_line.Substring(s);
                indexes[2*num]=s;
                coord[num]="";
                while((lower_line[s]=='-'||char.IsDigit(lower_line[s])||lower_line[s]=='.')&&lower_line[s]!='\0') {
                    coord[num]+=lower_line[s];
                    s++;
                }
                //Record where we are between the two numbers (so we know what delimiters there are).
                p[(2*num)+1]=lower_line.Substring(s);
                indexes[(2*num)+1]=s;
            }
            // Convert the extracted numbers in to floating point numbers and get the integer cell values.
            try {
                fx=Convert.ToSingle(coord[0]);
                fy=Convert.ToSingle(coord[1]);
            } catch { return false; }

            int_x=(int)(fx/8192);
            int_y=(int)(fy/8192);

            // Negative co-ordinates need to be decremented by 1.
            // e.g. (-6000, -12000) exists in cell (-1, -2) not (-0, -1) !

            if(fx<0) int_x-=1;
            if(fy<0) int_y-=1;
            // If this command is an AIFollow or AIEscort command and the X,Y co-ords are both 0,
            // do not make any changes.
            if(follow&&fx==0&&fy==0) return false;
            // If CheckCellChangeList() returns true then the co-ordinates pointing to this cell
            // should be modified, so calculate the offset and generate a modified version of the
            // Script command.
            if(CheckCellChangeList(int_x,int_y,out trans_x,out trans_y)) {
                new_fx=fx+(8192*trans_x);
                new_fy=fy+(8192*trans_y);

                Log("\n\tMODIFIED script usage of co-ords ("+fx.ToString()+", "+fy.ToString()+") to ("+
                    new_fx.ToString()+", "+new_fy.ToString()+")");
                // Reformulate the new line. Firstly everything up to the beginning of the first number.
                new_line=line.Substring(0,indexes[0]);
                // Then the first number, then everything between the two numbers (delimiters etc.)
                new_line+=new_fx.ToString();
                new_line+=line.Substring(indexes[1],indexes[2]-indexes[1]);
                // Then the second number, followed by the remainder of the original line.
                new_line+=new_fy.ToString();
                new_line+=line.Substring(indexes[3]);
                // Copy the new line over the original line.
                line=new_line;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes a new cell to the Rules array
        /// </summary>
        /// <remarks>
        /// Called on the first pass, this routine dumps a list of all individual cells that
        /// will need changing. The temporary file dumped is only used by ProcessDODTData() at
        /// present, but may be used to determine which script entries need modifying in future.
        /// </remarks>
        /// <param name="old_x">The cells old x position</param>
        /// <param name="old_y">The cells old y position</param>
        /// <param name="trans_x">The cells new x position</param>
        /// <param name="trans_y">The cells new y position</param>
        private static void WriteCellChangeList(int old_x,int old_y,int trans_x,int trans_y) {
            ChangedCell r=new ChangedCell();
            r.OldX=old_x;
            r.OldY=old_y;
            r.NewX=trans_x;
            r.NewY=trans_y;
            CellChangedList.Add(r);
        }

        /// <summary>
        /// Determines whether a particular cell will be modified.
        /// </summary>
        /// <remarks>
        /// This is useful for the Interior->Exterior door teleport references as well as scripts.
        /// </remarks>
        /// <param name="old_x">cells current x pos</param>
        /// <param name="old_y">cells current y pos</param>
        /// <param name="trans_x">the new x pos</param>
        /// <param name="trans_y">the new y pos</param>
        /// <returns>true to change cell, false otherwise</returns>
        private static bool CheckCellChangeList(int old_x,int old_y,out int trans_x,out int trans_y) {
            foreach(ChangedCell cc in CellChangedList) {
                if(cc.OldX==old_x&&cc.OldY==old_y) {
                    trans_x=cc.NewX;
                    trans_y=cc.NewY;
                    return true;
                }
            }
            trans_x=0;
            trans_y=0;
            return false;
        }

        /// <summary>
        /// Dumps a cell to a file
        /// </summary>
        /// <remarks>
        /// If '-l' was specified on the command-line then this procedure is
        /// called for each new cell found. A simple dump is made of the cell name
        /// and it's location; one line as a '#' comment and another as the raw
        /// values (easier for someone to turn in to a TESfaith rules file).
        /// </remarks>
        private static void LogCell(CellType cellType) {
            if(internal_list_mode) {
                CellList.Add(new ChangedCell(Cell.current_x,Cell.current_y,Cell.name,Cell.region_name));
            } else {
                FileStream fs=File.Open("CellDump.txt",FileMode.Append);
                StreamWriter sw=new StreamWriter(fs);

                if (cellType == CellType.Exterior)
                {
                    sw.WriteLine("(" + Cell.current_x.ToString() + "," + Cell.current_y.ToString() + ")\t" + (Cell.name != "" ? "\"" + Cell.name + "\"" : "-") + "\t\"" +
                        Cell.region_name + "\"\n");
                }
                else
				{
                    if (Cell.region_name != "")
                    {
                        sw.WriteLine("(INT-EXT)\t\"" + Cell.name + "\"\t\"" + Cell.region_name + "\"\n");
                    }
                    else
                    {
						sw.WriteLine("(INT)\t\"" + Cell.name + "\"\t" + "-" + "\n");
					}
				}

                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// Checks if any of the rules effect the current cell
        /// </summary>
        /// <remarks>
        /// CellRecord must be null unless dealing with an interior cell
        /// </remarks>
        /// <param name="CellRecord">The cell record</param>
        private static void CheckRules(ByteStream CellRecord) {
            CellData cell=Cell;
            bool Matched;
            bool Delete=false;
            bool Copy=false;
            bool Ignore=false;
            bool Changed=false;
            foreach(Rule r in Rules) {
                if(CellRecord==null) { //Exterior
                    if(r.type2==RuleType2.IntCopy||r.type2==RuleType2.IntDelete||r.type2==RuleType2.IntIgnore)
                        continue;
                    Matched=false;
                    switch(r.type) {
                        case RuleType.All:
                            Matched=true;
                            break;
                        case RuleType.Region:
                            if(cell.region_name==r.regionName) Matched=true;
                            break;
                        case RuleType.Cell:
                            if (cell.name == r.cellName)
                            {
                                Matched = true;
                            }
                            else if (r.data.OldX == cell.current_x && r.data.OldY == cell.current_y)
                            {
                                Matched = true;
                            }
                            break;
                    }
                    if(Matched) {
                        if(pass==1)
                            Log("Rule matched to exterior CELL ("+(Cell.name!=""?Cell.name + " - ":"")+Cell.current_x.ToString()+", "+
                                cell.current_y.ToString()+") - ");
                        switch(r.type2) {
                            case RuleType2.Delete: 
                                Delete=true;
                                if(pass==1) Log("DELETE");
                                break;
                            case RuleType2.Ignore: 
                                Ignore=true;
                                if(pass==1) Log("IGNORE");
                                break;
                            case RuleType2.Copy: 
                                Copy=true;
                                if(pass==1) Log("COPY");
                                break;
                            case RuleType2.Transpose:
                                Changed=true;
                                cell.new_x+=r.data.NewX;
                                cell.new_y+=r.data.NewY;
                                if(pass==1) Log("TRANSPOSE");
                                break;
                            case RuleType2.Move:
                                Changed=true;
                                cell.new_x=r.data.NewX;
                                cell.new_y=r.data.NewY;
                                if(pass==1) Log("MOVE");
                                break;
                        }
                        if(pass==1) Log("\n");
                    }
                } else { //Interior
                    if(!(r.type2==RuleType2.IntCopy||r.type2==RuleType2.IntDelete||r.type2==RuleType2.IntIgnore))
                        continue;
                    if(r.cellName==cell.name) {
                        Log("Rule matched to interior CELL \""+cell.name+"\" - ");
                        switch(r.type2) {
                            case RuleType2.IntDelete:
                                Delete=true;
                                if(pass==1) Log("DELETE");
                                break;
                            case RuleType2.IntIgnore:
                                Ignore=true;
                                if(pass==1) Log("IGNORE");
                                break;
                            case RuleType2.IntCopy:
                                CellRecord.WriteString(8,r.newName);
                                CellRecord.WriteByte(8+r.newName.Length,0);
                                CellRecord.WriteInt(4,r.newName.Length+1);
                                Copy=true;
                                Changed=true;
                                if(pass==1) Log("COPY");
                                break;
                        }
                        if(pass==1) Log("\n");
                    }
                }
            }
            if(Ignore) return;
            if(Delete) {
                total_records_changed++;
                Cell.copy=false;
                Cell.save=false;
                return;
            }
            if(Changed) {
                total_records_changed++;
                Cell=cell;
                Cell.copy=true;
                if(Copy) Cell.save=true; else Cell.save=false;
            }
        }

        private static void Log(string Message) {
            if(generate_log&&!list_cell_mode) writer.Write(Message);
        }
    }
}
