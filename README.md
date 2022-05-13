# DW1--font-editor
Font editor for EUR and JAP fonts of Digimon Worlds PSX

This is a font editor for the videogame Digimon World for the PSX.

You need to open a ".1bppf" which is an arbitrary extension for a binary file copying the font of the game's executable.
For now, the tool only supports fonts of the EUR and JAP versions of the game.
The tool will read from the table provided, and write as is, if you want to modify any character in the table you can do so manually. The tool will load whatever table the font file has, and will save it correctly.

You can find the original fonts at these offsets:
- JAP -> 00098578
- EUR -> 00098848

IMPORTANT - It is needed to save each glyph after editing, or the changes wont be saved.
Also, if the tool crashes for any reaso, it'll always leave behind a temp file, you can use it to save your progress.

Thanks to:
-Romsstar and Syd, true legends for anyone interested in this game.
-CUE for finding all the font data for the different versions of the game, and explaining how the font worked.
-The DMC(Digimon Modding Community) discord: https://discord.gg/cb5AuxU6su


Proyected updates:
-Add support for USA fonts.
-Translate public version to english. SORRY FOR NOW (Yeah, really should've tought of that ^^U)
