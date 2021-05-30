# Przetwarzanie zamówieñ

>Autorzy: TS + PW

## Wstêp

Celem jest przygotowanie systemu przetwarzania p³atnoœci, zawierajcego dwa modu³y: `Payments` (p³atnoœci) i `Shipment` (dostawa).
Oczywiœcie w praktyce ca³y kod znajduje siê w jednym projekcie, ale podejdŸ do jego implementacji tak, jakby poszczególne modu³y by³y oddzielnie opracowywane przez odrêbne zespo³y.

## Przetwarzanie zamówieñ

Dane dotycz¹ce zamówieñ s¹ przechowywane w dwóch odrêbnych bazach danych `LocalOrdersDB` i `GlobalOrdersDB`.
Potencjalne zmiany w ich strukturze nie powinny w ¿aden sposób wp³ywaæ na "klienta" (`Program.cs`).
Ponadto, system powinien przetwarzaæ wszystkie zamówienia razem, niezale¿nie od docelowego kraju.
Zamówienie (`Order`) zawiera informacjê o wybranych metodach p³atnoœci: `SelectedPayments`.
Z ka¿d¹ p³atnoœci¹ (`Payment`) jest zwi¹zana konkretna metoda p³atnoœci oraz maksymalna kwota, jaka mo¿e zostaæ zap³acona z jej u¿yciem.
System ma wspieraæ trzy metody p³atnoœci: `PayPal`, `Invoice`, `CreditCard`.
Musi jednak byæ zaprojektowany tak, aby w przysz³oœci ³atwo mo¿na by³o dodaæ obs³ugê kolejnych (dodanie nowej metody p³atnoœci nie powinno wymagaæ modyfikacji istniej¹cych).
Przetwarzanie p³atnoœci polega na próbie zap³aty maksymalnej dopuszczalnej kwoty kolejno dla ka¿dej metody p³atnoœci zwi¹zanej z zamówieniem, dopóki nie zostanie osi¹gniêta ³¹czna kwota zamówienia.
Kolejnoœæ metod p³atnoœci jest zawsze taka sama: `PayPal` -> `Invoice` -> `CreditCard`.

Próba p³atnoœci dan¹ metod¹ mo¿e zakoñczyæ siê niepowodzeniem, zaimplementowanym w nastêpuj¹cy sposób:
 - Co trzecia p³atnoœæ `Invoice` koñczy siê niepowodzeniem.
 - Ka¿da p³atnoœæ `PayPal` ma 30% szans na niepowodzenie (`Random` zainicjalizowany ziarnem `1234`).
 - P³atnoœci `CreditCard` zawsze koñcz¹ siê powodzeniem.

W przypadku niepowodzenia, metoda p³atnoœci jest pomijana i przetwarzanie przechodzi do kolejnej.
Pomyœlnie zakoñczone p³atnoœci powinny byæ dodane do `FinalizedPayments` wraz z zap³acon¹ kwot¹.

Ponadto, w trakcie przetwarzania zamówienia nale¿y odpowiednio ustawiæ jego `Status`:
 - `WaitingForPayment` gdy jest nieop³acone,
 - `PaymentProcessing` gdy jest czêœciowo op³acone,  
 - `ReadyForShipment` gdy jest op³acone i mo¿e zostaæ dostarczone (trafiæ do modu³u `Shipment`)

### Wymagania dot. modu³u Payments

- Dodanie nowych metod p³atnoœci powinno byæ mo¿liwe bez zmian istniej¹cych komponentów `PaymentMethods` components, za wyj¹tkiem enum-a `PaymentMethod`.
- Zmiana jednej metody p³atnoœci nie powinna poci¹gaæ za sob¹ jakichkolwiek modyfikacji pozosta³ych.
- Je¿eli ³¹czna kwota zamówienia zosta³a ju¿ zap³acona, nie przechodzimy ju¿ do kolejnych metod p³atnoœci.
- Nale¿y u¿yæ komponentów dostarczonych w zadaniu.

## Dostarczanie zamówieñ

Dodaj filtr, aby uwzglêdniæ tylko op³acone zamówienia (o statusie `ReadyForShipment`) i zaimplementuj dla nich proces dostarczania przesy³ek.
Wypisz etykiety dla ka¿dego zamówienia i dla ka¿dej przesy³ki z³o¿onej z zamówieñ.
System ma wspieraæ dwóch dostawców: `LocalPost` i `Global`.
Musi jednak byæ zaprojektowany tak, aby w przysz³oœci ³atwo mo¿na by³o dodaæ obs³ugê kolejnych (dodanie nowego dostawcy nie powinno wymagaæ modyfikacji istniej¹cych).

Wybór dostawcy (`ShipmentProvider`):
 - `LocalPost` obs³uguje wszystkie przesy³ki adresowane do Polski.
 - `Global` obs³uguje wszystkie przesy³ki zagraniczne.

Obliczanie podatku:
 - Baza danych `TaxRatesDB` zawiera informacje o stawkach podatku VAT w poszczególnych krajach.
 - Na potrzeby zadania zak³adamy, ¿e jest dostarczana przez zewnêtrzn¹ firmê, w zwi¹zku z czym jej struktura nie jest znana klasie ShipmentProvider (i w przysz³oœci mo¿e siê zmieniæ).
 - Podatek jest obliczany jako X procent z `PaidAmount`, gdzie X jest stawk¹ VAT dla danego kraju pobran¹ z bazy danych.

Etykiety i paczkowanie:
 - Poszczególni dostawcy (`ShipmentProvider`) maj¹ w³asn¹ logikê generowania etykiet oraz w ró¿ny sposób ³¹cz¹ zamówienia w przesy³ki.
 - `LocalPost`: jedna przesy³ka (`IParcel`) dla wszystkich zamówieñ.
 - `Global`: oddzielne przesy³ki dla ka¿dego kraju (`Recipient.Country`).

Najpierw zarejestruj u w³aœciwego dostawcy wszystkie op³acone i gotowe do wysy³ki zamówienia. Dla ka¿dego z nich wypisz etykietê odpowiedni¹ dla dostawcy (`Printer.PrintLabel`).
Skorzystaj z interfejsu `ILabelFormatter`, aby zapewniæ mo¿liwoœæ ponownego wykorzystania logiki dla wypisywania etykiet w przysz³oœci.
Etykiety dla dostawcy `LocalPost` nie podaj¹ docelowego kraju, na przyk³ad:
```
Shipment provider: LocalPost
Janina Osiwiecka
Kamionka 3/34
Lipsko
25-895
```

Etykiety dla dostawcy `Global` uwzglêdniaj¹ kraj adresata, na przyk³ad:
```
Shipment provider: Global
Lea Mathias
9054 Share-wood
Manhattan
90561
USA
```

Nastêpnie, wypisz podsumowanie dla ka¿dej przesy³ki (`IParcel`) utworzonej przez dostawców w poprzednim kroku.
Preferowany format:
```
Shipment provider: Global
TotalPrice:     246,00, TotalTax:       0,00, TotalPriceWithTax:     246,00
--------------------------------------------------------
-------------------------Hawaii-------------------------
 OrderId          Amount             Tax   AmountWithTax
       1          246,00            0,00          246,00
TOTALS:           246,00                          246,00

--------------------------------------------------------
```

### Wymagania dot. modu³u Shipments

- Dostawcy przesy³ek s¹ "ciê¿kimi" obiektami, wiêc zastosuj leniw¹ inicjalizacjê: dopiero wówczas, gdy pojawi siê pierwsze zamówienie do zarejestrowania u danego dostawcy.
- Do wypisania podsumowania przesy³ek zastosuj dostarczony interfejs `ISummaryFormatter` wraz z domyœln¹ implementacj¹ `SummaryFormatter`.
- Dodanie nowych dostawców (np. dla poszczególnych krajów) nie powinno wymagaæ ¿adnych zmian w "kliencie", czyli w `Program.cs`.
- Dodanie nowych dostawców nie powinno te¿ wi¹zaæ siê z ¿adnymi zmianami ju¿ istniej¹cych.
- Zmiana struktury bazy stawek podatku VAT nie powinno wymagaæ ¿adnych zmian logiki w obrêbie modu³u Shipments.


## Uwagi
- Obydwa modu³y powinny wypisywaæ informacje na konsolê (przyk³ad w pliku `output.txt`).