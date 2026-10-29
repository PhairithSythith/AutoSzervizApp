# AutoSzervizApp
ASP.NET Core MVC alapú autószerviz-kezelő alkalmazás

## Cél

Az alkalmazás célja, hogy támogassa az ügyfelek, járművek, szervizidőpontok és munkalapok kezelését.

## Tervezett technológiák

- C# (backend logika)
- ASP.NET Core MVC (webes keretrendszer)
- Entity Framework Core (adatbázis-kezelés)
- SQLite (adatbázis)
- ASP.NET Core Identity (bejelentkezés és jogosultságok)
- Razor nézetek és HTML (felhasználói felület)

## Tervezett fejlesztési lépések

- [ ] Alap projekt létrehozása
- [ ] Adatmodell és adatbázis
- [ ] Bejelentkezés és jogosultságok
- [ ] Ügyfelek és járművek kezelése
- [ ] Szervizidőpontok és jóváhagyás
- [ ] Munkalapok és tételek
- [ ] Dokumentáció és ellenőrzés

## Állapot

A projekt újraépítése 2026. október 28-án kezdődött. 

## Architektúra és működés (MVC)
- Models: Az adatszerkezeteket és az adatbázis tábláit írják le (ügyfelek, járművek, munkalapok).
- Views: A böngészőben megjelenő felületek és űrlapok.
- Controllers: A kéréseket feldolgozó és az üzleti logikát irányító vezérlők.

## Felhasználói szerepkörök és jogosultságok
A rendszer két fő szerepkört különböztet meg:
1. Admin: Teljes hozzáféréssel rendelkezik.
2. Employee (Alkalmazott): A napi szervizmunkát végzi.

## Továbbfejlesztési ötletek
- Online időpontfoglalási felület fejlesztése az ügyfelek számára.
- Ügyféloldali portál kialakítása, ahol a tulajdonosok élőben láthatják a saját járművük szervizelési állapotát.
- Részletes módosítási napló (Audit Log) bevezetése a teljes változástörténet nyomon követésére.
- E-mailes értesítések küldése a státuszváltozásokról (pl. amikor elkészült az autó).
- Statisztikai kimutatások és vezetői dashboard készítése a műhely működésének elemzéséhez/ellenőrzéséhez.