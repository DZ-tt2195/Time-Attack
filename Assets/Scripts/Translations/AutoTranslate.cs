public static class AutoTranslate 
{ 

public static string Start_on_Wave (string Num) => Translator.inst.Translate("Start_on_Wave", new() {("Num", Num)});

public static string Best_Score (string Num) => Translator.inst.Translate("Best_Score", new() {("Num", Num)});

public static string Difficulty (string Num) => Translator.inst.Translate("Difficulty", new() {("Num", Num)});

public static string Bullets_Missed (string Num) => Translator.inst.Translate("Bullets_Missed", new() {("Num", Num)});

public static string Health_Lost (string Num) => Translator.inst.Translate("Health_Lost", new() {("Num", Num)});

public static string Wave (string Num,string Max) => Translator.inst.Translate("Wave", new() {("Num", Num),("Max", Max)});

public static string Skipped_Ahead (string Num) => Translator.inst.Translate("Skipped_Ahead", new() {("Num", Num)});

public static string Enemies (string Num,string Max) => Translator.inst.Translate("Enemies", new() {("Num", Num),("Max", Max)});

public static string Bullets (string Num,string Max) => Translator.inst.Translate("Bullets", new() {("Num", Num),("Max", Max)});

public static string Health (string Num,string Max) => Translator.inst.Translate("Health", new() {("Num", Num),("Max", Max)});

public static string Score (string Num) => Translator.inst.Translate("Score", new() {("Num", Num)});

public static string FPS (string Num) => Translator.inst.Translate("FPS", new() {("Num", Num)});

public static string Description() => Translator.inst.Translate("Description");
public static string Designer() => Translator.inst.Translate("Designer");
public static string Last_Update() => Translator.inst.Translate("Last_Update");
public static string Translator_Credit() => Translator.inst.Translate("Translator_Credit");
public static string Language() => Translator.inst.Translate("Language");
public static string Choose_Level() => Translator.inst.Translate("Choose_Level");
public static string Controls() => Translator.inst.Translate("Controls");
public static string Play() => Translator.inst.Translate("Play");
public static string Delete() => Translator.inst.Translate("Delete");
public static string Replay() => Translator.inst.Translate("Replay");
public static string Quit() => Translator.inst.Translate("Quit");
public static string Paused() => Translator.inst.Translate("Paused");
public static string Restart() => Translator.inst.Translate("Restart");
public static string No_Score() => Translator.inst.Translate("No_Score");
public static string Victory() => Translator.inst.Translate("Victory");
public static string Lost() => Translator.inst.Translate("Lost");
public static string Resupply() => Translator.inst.Translate("Resupply");
public static string Health_Pack() => Translator.inst.Translate("Health_Pack");
public static string Juggle_Balls() => Translator.inst.Translate("Juggle_Balls");
public static string Infinite_Bullets() => Translator.inst.Translate("Infinite_Bullets");
public static string Tutorial_1() => Translator.inst.Translate("Tutorial_1");
public static string Tutorial_2() => Translator.inst.Translate("Tutorial_2");
public static string Enemy_Rush() => Translator.inst.Translate("Enemy_Rush");
public static string Random() => Translator.inst.Translate("Random");
public static string Endless() => Translator.inst.Translate("Endless");
public static string First_Tutorial() => Translator.inst.Translate("First_Tutorial");
public static string Second_Tutorial() => Translator.inst.Translate("Second_Tutorial");
public static string Third_Tutorial() => Translator.inst.Translate("Third_Tutorial");
public static string Fourth_Tutorial() => Translator.inst.Translate("Fourth_Tutorial");
public static string Update_History() => Translator.inst.Translate("Update_History");
public static string Blank() => Translator.inst.Translate("Blank");
public static string Update_0() => Translator.inst.Translate("Update_0");
public static string Update_0_Text() => Translator.inst.Translate("Update_0_Text");
public static string Update_1() => Translator.inst.Translate("Update_1");
public static string Update_1_Text() => Translator.inst.Translate("Update_1_Text");
public static string Update_2() => Translator.inst.Translate("Update_2");
public static string Update_2_Text() => Translator.inst.Translate("Update_2_Text");
public static string Update_3() => Translator.inst.Translate("Update_3");
public static string Update_3_Text() => Translator.inst.Translate("Update_3_Text");
public static string Upload_Translation() => Translator.inst.Translate("Upload_Translation");
public static string Download_English() => Translator.inst.Translate("Download_English");
public static string Update_4() => Translator.inst.Translate("Update_4");
public static string Update_4_Text() => Translator.inst.Translate("Update_4_Text");
public static string Update_5() => Translator.inst.Translate("Update_5");
public static string Update_5_Text() => Translator.inst.Translate("Update_5_Text");
}
public enum ToTranslate {
Description,Designer,Last_Update,Translator_Credit,Language,Choose_Level,Controls,Play,Delete,Replay,Quit,Paused,Restart,No_Score,Victory,Lost,Resupply,Health_Pack,Juggle_Balls,Infinite_Bullets,Tutorial_1,Tutorial_2,Enemy_Rush,Random,Endless,First_Tutorial,Second_Tutorial,Third_Tutorial,Fourth_Tutorial,Update_History,Blank,Update_0,Update_0_Text,Update_1,Update_1_Text,Update_2,Update_2_Text,Update_3,Update_3_Text,Upload_Translation,Download_English,Update_4,Update_4_Text,Update_5,Update_5_Text
}
