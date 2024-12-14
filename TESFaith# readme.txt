*********************************************************************
** A graphical utility for manipulating cells in morrowind plugins **
** Author: Timeslip (Original TESFaith by Paul Halliday)           **
*********************************************************************

TESFaith was a utility written by Paul Halliday (aka Lightwave,) that can be used to move/copy/delete individual cells, entire regions or the contents of whole plugins. Unfortunately it was a console application, and a lot of the otherwise great modders over at the elder scrolls forums wouldn't know what a dos prompt was if it bit them. So I bring you all TESFaith#: TESFaith with a spangaly new GUI, nice big easy to use buttons and about 10x the memory usage. Enjoy.

WARNING: This is a _BETA_ version. Most of the code is a direct port of the original TESFaith, but I make no guarantee’s that I haven't buggered something up while porting it.

This is a .NET application. You need .NET v1.1 to run it. Download either from windows update, or from this oversized url:
http://www.microsoft.com/downloads/details.aspx?FamilyID=262d25e3-f589-4842-8157-034d1e7cf3a3&DisplayLang=en

***********
** Usage **
***********
I recommend you go away and read the original TESFaith readme now, before carrying on here. These are only quick notes, and I don't intend to explain everything. (Of course, you can ignore anything about config files or command line parameters. They're not needed here.)

Running TESFaith.exe will bring up a grid, along with a few buttons. Use the open mod button to open a mod that you want to work on. Close mod will close it again. Note that any changes you make are saved as you go along, so there's no save button. Any time a change is made, changes are saved to the _original_ file, (TESFaith created a new file,) and a backup file called [pluginname].esp.xxx is created, where [pluginname] is the original plugin name, and xxx is a 3 digit number. (ie, it uses exactly the same backup scheme as MWEdit.)

The overlay button displays all cells contained in one or more plugins in a different colour, so you can easily see where they overlap with the currently open plugin. You can have as many overlays as you like, but clear overlay will clear them all. It wont give you any choice about which ones to clear.

List cells will create a text file containing a list of all cells contained in the open plugin. It will be called 'CellDump.txt' and will be in the same directory as TESFaith.exe.

Batch process will bring up a dialog that works in the same was as the original TESFaith. All the original command line options are available as check boxes. Everything there should be self explanatory, but there are a couple of things I need to mention that differ from the original TESFaith. First there's a new rule type - ignore. Any cell that comes under an ignore rule will not be effected by any other rules. Second, copy must now be combined with either move or translate, as it doesn't let you set any new co-ordinates. If you use copy by itself, a copy of the cell will be generated in exactly the same position as the original. Lastly, multiple rules can now effect a single cell. The order in which rules are applied are: ignore, delete, move and translate, copy. (ie, if a cell is effected by ignore and delete, it will be ignored. If it is effected by delete and move, it will be deleted.) You can use multiple translates on one cell, but using multiple moves, or combining translates and moves, will have weird side effects. Region and cell names are case sensitive.

The left/right/up/down buttons are used for scrolling around the map.

The next three buttons allow you to modify single cells without having to create your own rules. Moving the mouse around the map will give the co-ordinates of each cell, along with the cell and region names given in the active or overlaid plugins. Clicking on a cell will cause a red box to appear around it. You can then click on delete to delete that cell. If you click on copy or move you must then click on a second cell to move it to. Right clicking will cancel the selection without making any changes to the plugin.

The save button will output a colour coded map showing which cells are modified by all overlaid plugins. Note that any active plugins wont appear on this map at all. 

***************
** changelog **
***************

v0.1
Added: An option to output map files containing a colour-keyed map of the location of up to 63 mods
Tweak: Using the copy/move buttons will now update scripts as well as just the LAND/CELL data
Fixed: When overlaying cells, they no longer get duplicated internally.
Fixed: When moving the mouse pointer over cells overlaid with more than one plugin, all plugins are now shown.

*************
** Credits **
*************
Paul Halliday, obviously. This program could never have existed without the original TESFaith.

lochnarus: for giving me an excuse to do another week of procrastination instead of getting any real work done. (And almost making me forget to do my theoretical physics homework too :s )

There's a couple of people listed in the original TESFaith readme too. I've included the whole thing with this utility because there's some useful info in it.

Just like the original TESFaith, this program is open source. Do what you will with it, but remember to credit Paul Halliday and me somewhere.