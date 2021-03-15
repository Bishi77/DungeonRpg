Jellemző: DungeonGenerator
	Fejlesztőként szükség van egy olyan modellre, 
	ami létrehozza a bejárható térképet, kezdő, végpontokkal, 
	falakkal, utakkal, szörnyekkel, stb.

Forgatókönyv: Pálya generálás
Adott egy DungeonGenerator példány
Ha létrehozzuk a Dungeon példányt 3 rows és 3 columns méretben
Akkor a Dungeon.Leveldata mérete 3 és 3 lesz.
Ha hozzáadunk egy utat az 1 1 pozícióba
Akkor nem lehet fal az 1 1 pozíción
Ha hozzáadunk megint egy utat az 1 1 pozícióba
Akkor az 1 1 pozícióban csak 1 út lehet, és más nem