================================================================================
                              THE LAST BOUNTY
================================================================================

Vítejte u hry The Last Bounty! 

Poznámka: Hra se původně měla jmenovat "Wanted-Dead or alive", proto se tak 
stále jmenuje spouštěcí soubor.

Ačkoliv projekt ještě není stoprocentně kompletní (tutoriál, vedlejší úkoly a 
některé animace nejsou plně dotažené a pár věcí chybí), už v tomto stavu má 
hra rozhodně co nabídnout a můžete si ji naplno užít. 

Rád bych také dodal, že na projektu i nadále pokračuji a usilovně pracuji 
na tom, aby byla hra do budoucna plně funkční a dotažená do konce.

--------------------------------------------------------------------------------
JAK HRU STÁHNOUT A SPUSTIT
--------------------------------------------------------------------------------

Jelikož je hra větší a GitHub neumožňuje nahrávat soubory nad 
100 MB, musela být rozdělena do dvou archivů. Pro správné spuštění prosím 
postupujte takto:

1. Stáhněte si z tohoto repozitáře oba soubory: build.part1.rar a build.part2.rar.
2. Ujistěte se, že máte oba soubory stažené do stejné složky.
3. Otevřete složku ve WinRARu a klávesovou zkratkou Ctrl + levé tlačítko myši 
   vyberte oba buildy. Poté zvolte možnost „Extrahovat do určené složky“ a 
   vyberte jakoukoli prázdnou složku.
4. Otevřete složku s extrahovaným buildem a spusťte soubor "Wanted-Dead or alive.exe".

--------------------------------------------------------------------------------
OVLÁDÁNÍ A HERNÍ MECHANISMY
--------------------------------------------------------------------------------

Jelikož herní tutoriál ještě není plně implementován, zde je přehled všeho, 
co budete k přežití potřebovat:

!!! DŮLEŽITÉ UPOZORNĚNÍ PRO START !!!
Aby bylo možné ve hře postoupit, MUSÍTE si hned na začátku koupit revolver 
u zbrojaře, který je v každém městě. Bez něj se nedostanete dál!

Nezapomeňte si také hlídat zásoby! Jídlo a pití můžete kdykoliv zakoupit 
u zelináře, kterého najdete rovněž v každém městě.

[ Základní pohyb a akce ]
* W, A, S, D - Pohyb postavy
* Shift - Sprint
* Mezerník - Skok
* H (Čutora) - Napití z čutory. Jakmile je prázdná, můžete ji znovu naplnit 
  u jakékoliv studny ve hře pomocí klávesy E.

[ Interakce, nákupy a inventář ]
* E (Interakce) - Pokud uvidíte poletující písmeno "E", znamená to, že můžete 
  s objektem před vámi interagovat. Tímto způsobem se spouští dialogy s NPC, 
  doplňuje voda ze studny nebo hrají různé minihry (střelnice, kostky).
* B (Batoh) - Otevření inventáře. Během hry můžete sbírat, kupovat a vyhazovat 
  různé předměty.

[ Duely ]
* Ve hře narazíte na mechaniku duelů. Pokaždé se v náhodném městě každou chvíli 
  objeví duelista. 
* Ovládání duelu: Na začátku duelu musíte DRŽET pravé tlačítko myši. Jakmile 
  zazní signál, bleskově pravé tlačítko pusťte a stiskněte levé tlačítko pro výstřel.

[ Mapa a svět ]
* M (Mapa) - Otevření mapy, která vám ukáže, kde se zrovna nacházíte a jaké 
  jsou vaše aktivní úkoly.
* Vlak - Po celé mapě jezdí dokola vlak. Nastupovat do něj lze POUZE u dveří 
  a jakmile se vlak rozjede, nedá se z něj vystoupit. Dávejte si obrovský 
  pozor na zastávkách! Vlak má neviditelnou kolizní zónu širší než 
  jeho model, takže vás může nečekaně "přejet" a zabít, i když i když stojite jen blízko tratě.

[ Systém ukládání a Menu ]
* F1 - Uložení hry
* F2 - Načtení uložené hry
* F3 - Smazání uložené hry
* F4 - Otevření herního menu.

--------------------------------------------------------------------------------
TIPY K HLAVNÍM ÚKOLŮM (PŘÍBĚH)
--------------------------------------------------------------------------------

Herní "story manažer" momentálně nevypisuje úplně přesné instrukce a lokace 
k plnění hlavních úkolů. Zde je upřesnění, kam přesně musíte jít:

* Úkol Karbaník: V zadání není uvedena lokace. Abyste ho mohli porazit, 
  musíte se vydat do "Bandit campu".
* Úkol Učenec: Situace je stejná jako u Karbaníka. Učence najdete v lokaci 
  "Desert outpost".
* Úkol Pravá ruka El Diabla: Tohoto bosse najdete v lokaci "Ghost town".
* Finální úkol El Diablo: V instrukcích chybí přesná poloha. El Diabla 
  najdete schovaného mezi horami na jihovýchodě mapy.

--------------------------------------------------------------------------------
HERNÍ CHYBY A OMEZENÍ
--------------------------------------------------------------------------------

V aktuální verzi se nachází pár chyb a technických omezení. Dejte si pozor zejména na tyto:

* Pevné rozlišení obrazovky: Ve hře zatím nefunguje rozhraní pro různá rozlišení. 
  Hra je aktuálně stavěná a optimalizovaná pouze na rozlišení 2560x1440. Při 
  jiném nastavení monitoru se může uživatelské rozhraní zobrazovat nekorektně.
* Ovládání hlasitosti: V herním nastavení momentálně nefunguje posuvník pro 
  změnu hlasitosti (volume). 
* Pravá ruka El Diabla (Prohra): Pokud s tímto bossem prohrajete duel, ztratíte 
  šanci úkol dokončit. Bandita z nějakého důvodu po prohře zmizí (správně by 
  měl mizet až po vaší výhře). Důrazně doporučujeme hru před duelem ULOŽIT (F1)!
* Vedlejší úkoly (Spawnování): Vedlejší úkoly můžete přijmout u šerifa 
  v kanceláři a následně se vám zobrazí na mapě. Aktuálně se ale cíle úkolů 
  spawnují na zcela náhodných místech po světě a ne tam, kde by logicky měly být.
* Kamera u duelů: U některých duelistů je špatně nastavené místo pro kameru, 
  takže se při souboji můžete dívat skrz textury pod mapu.
* Záhadná úmrtí na nerovném terénu: Občas se může stát, že vaše postava při 
  chůzi přes nerovnosti nebo kopce zničehonic zemře. Doporučujeme hru často 
  ukládat (klávesa F1)!
