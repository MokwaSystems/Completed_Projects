# FOR VICTORY

Mój stary i jeden z pierwszych skończonych i zrobionych na zlecenie projektów gry.
Proste wyścigii które polegały na ściganiu się z "AI" przez jak najszybsze kliklanie strzałki w górę.
W sumie to nic wielkiego i na pewno nic odkrywczego. Jednak gra mimo prostych zasad 
i prostej mechaniki "na papierze" była na swój sposób ciekawa. Sam zlecony projekt zawierał ciekawe założenia: 

==> Gracz miał zawsze wygrywać (oczywiście w "głupich" sytuacjach, specjalnie "prowokowanych" przez użytkownika
aby np. ośmieszyć system gry - wtedy silnik gry miał się tego domyśleć i gracz przegrywał)

==> Możliwość określenia presonalizacji swojego samochodu. Bardzo podstawowej ale jednak nadającej indywidualność

Ulepszyłem troszkę założenia:

==> Dodałem bezwzględne wygrywanie komputerowych przeciwników przy "ośmieszaniu" systemu gry
==> Oponenci i ich strategia wyścigu jest losowana przy starcie gry
==> Strategia jest podzielona na trzy fazy: Niektórzy z kierowców są szybcy już na początku,
inni dotrzymują prędkością "towarzystwa" graczowi. A jeszcze inni startują prawie równo z graczem
lub sporo wolniej i w jakimś momencie gry - zwalniają. Dają się wyprzedzić
==> EMOCJE - Zadaniem przeciwników było wymuszenie rywalizacji. Czyli wspomniane wyprzedzanie, zwalnianie,
trzymanie się blisko gracza czy bardzo szybka jazda prawie do końca wyścigu. Gracz nie może czuć się
"osamotniony" gdy rywalizuje.

Nauczyłem się dość dużo przy tym projekcie. To był jeden z tych ważnych - na których mi bardzo zależało
z jasno określonym terminem. Najbardziej spodobało mi się to, że użyłem w kodzie - jak to nazywam - 
strategii parametrów. Czyli dodałem zmienne które określały pewne parametry mające wpływ na np. poziom trudności
czy startu rywali, prędkości i jej kontroli i tym podobnych. Po napisaniu stabilnej wersji mogłem zmieniać tylko
te parametry i w prosty sposób "wyśróbować" pewne detale. Oczywiście to nie była jedyna sprawa której się nauczyłem
- muszę jeszcze wspomnieć o mechaniźmie obsługi kliknięcia i jeśli np. gracz nie klika (albo zaczął robić to wolniej)
pasek przyspieszenia (nie pokazany na ekranie - istniał w kodzie i w mojej głowie) - stopniowo zwalnia. Im szybciej
gracz wciskał klawisz - tym też ciężej było utrzymać maksymalną prędkość. Ponieważ to też się skalowało.

Projekt został wykorzystany przy szkoleniach dotyczących "grywalizacji". Spełnił swoje zadanie. 
Mimo dość, na pierwszy rzut oka - prostej mechaniki - zawiera w sobie parę detali które już do tych prostych
nie należą.

Autor projektu: Przemysław Mokwa
Zostały usunięte wszelakie grafiki mające związek z zleceniodawcą