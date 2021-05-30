# Przetwarzanie zam�wie�

>Autorzy: TS + PW

## Wst�p

Celem jest przygotowanie systemu przetwarzania p�atno�ci, zawierajcego dwa modu�y: `Payments` (p�atno�ci) i `Shipment` (dostawa).
Oczywi�cie w praktyce ca�y kod znajduje si� w jednym projekcie, ale podejd� do jego implementacji tak, jakby poszczeg�lne modu�y by�y oddzielnie opracowywane przez odr�bne zespo�y.

## Przetwarzanie zam�wie�

Dane dotycz�ce zam�wie� s� przechowywane w dw�ch odr�bnych bazach danych `LocalOrdersDB` i `GlobalOrdersDB`.
Potencjalne zmiany w ich strukturze nie powinny w �aden spos�b wp�ywa� na "klienta" (`Program.cs`).
Ponadto, system powinien przetwarza� wszystkie zam�wienia razem, niezale�nie od docelowego kraju.
Zam�wienie (`Order`) zawiera informacj� o wybranych metodach p�atno�ci: `SelectedPayments`.
Z ka�d� p�atno�ci� (`Payment`) jest zwi�zana konkretna metoda p�atno�ci oraz maksymalna kwota, jaka mo�e zosta� zap�acona z jej u�yciem.
System ma wspiera� trzy metody p�atno�ci: `PayPal`, `Invoice`, `CreditCard`.
Musi jednak by� zaprojektowany tak, aby w przysz�o�ci �atwo mo�na by�o doda� obs�ug� kolejnych (dodanie nowej metody p�atno�ci nie powinno wymaga� modyfikacji istniej�cych).
Przetwarzanie p�atno�ci polega na pr�bie zap�aty maksymalnej dopuszczalnej kwoty kolejno dla ka�dej metody p�atno�ci zwi�zanej z zam�wieniem, dop�ki nie zostanie osi�gni�ta ��czna kwota zam�wienia.
Kolejno�� metod p�atno�ci jest zawsze taka sama: `PayPal` -> `Invoice` -> `CreditCard`.

Pr�ba p�atno�ci dan� metod� mo�e zako�czy� si� niepowodzeniem, zaimplementowanym w nast�puj�cy spos�b:
 - Co trzecia p�atno�� `Invoice` ko�czy si� niepowodzeniem.
 - Ka�da p�atno�� `PayPal` ma 30% szans na niepowodzenie (`Random` zainicjalizowany ziarnem `1234`).
 - P�atno�ci `CreditCard` zawsze ko�cz� si� powodzeniem.

W przypadku niepowodzenia, metoda p�atno�ci jest pomijana i przetwarzanie przechodzi do kolejnej.
Pomy�lnie zako�czone p�atno�ci powinny by� dodane do `FinalizedPayments` wraz z zap�acon� kwot�.

Ponadto, w trakcie przetwarzania zam�wienia nale�y odpowiednio ustawi� jego `Status`:
 - `WaitingForPayment` gdy jest nieop�acone,
 - `PaymentProcessing` gdy jest cz�ciowo op�acone,  
 - `ReadyForShipment` gdy jest op�acone i mo�e zosta� dostarczone (trafi� do modu�u `Shipment`)

### Wymagania dot. modu�u Payments

- Dodanie nowych metod p�atno�ci powinno by� mo�liwe bez zmian istniej�cych komponent�w `PaymentMethods` components, za wyj�tkiem enum-a `PaymentMethod`.
- Zmiana jednej metody p�atno�ci nie powinna poci�ga� za sob� jakichkolwiek modyfikacji pozosta�ych.
- Je�eli ��czna kwota zam�wienia zosta�a ju� zap�acona, nie przechodzimy ju� do kolejnych metod p�atno�ci.
- Nale�y u�y� komponent�w dostarczonych w zadaniu.

## Dostarczanie zam�wie�

Dodaj filtr, aby uwzgl�dni� tylko op�acone zam�wienia (o statusie `ReadyForShipment`) i zaimplementuj dla nich proces dostarczania przesy�ek.
Wypisz etykiety dla ka�dego zam�wienia i dla ka�dej przesy�ki z�o�onej z zam�wie�.
System ma wspiera� dw�ch dostawc�w: `LocalPost` i `Global`.
Musi jednak by� zaprojektowany tak, aby w przysz�o�ci �atwo mo�na by�o doda� obs�ug� kolejnych (dodanie nowego dostawcy nie powinno wymaga� modyfikacji istniej�cych).

Wyb�r dostawcy (`ShipmentProvider`):
 - `LocalPost` obs�uguje wszystkie przesy�ki adresowane do Polski.
 - `Global` obs�uguje wszystkie przesy�ki zagraniczne.

Obliczanie podatku:
 - Baza danych `TaxRatesDB` zawiera informacje o stawkach podatku VAT w poszczeg�lnych krajach.
 - Na potrzeby zadania zak�adamy, �e jest dostarczana przez zewn�trzn� firm�, w zwi�zku z czym jej struktura nie jest znana klasie ShipmentProvider (i w przysz�o�ci mo�e si� zmieni�).
 - Podatek jest obliczany jako X procent z `PaidAmount`, gdzie X jest stawk� VAT dla danego kraju pobran� z bazy danych.

Etykiety i paczkowanie:
 - Poszczeg�lni dostawcy (`ShipmentProvider`) maj� w�asn� logik� generowania etykiet oraz w r�ny spos�b ��cz� zam�wienia w przesy�ki.
 - `LocalPost`: jedna przesy�ka (`IParcel`) dla wszystkich zam�wie�.
 - `Global`: oddzielne przesy�ki dla ka�dego kraju (`Recipient.Country`).

Najpierw zarejestruj u w�a�ciwego dostawcy wszystkie op�acone i gotowe do wysy�ki zam�wienia. Dla ka�dego z nich wypisz etykiet� odpowiedni� dla dostawcy (`Printer.PrintLabel`).
Skorzystaj z interfejsu `ILabelFormatter`, aby zapewni� mo�liwo�� ponownego wykorzystania logiki dla wypisywania etykiet w przysz�o�ci.
Etykiety dla dostawcy `LocalPost` nie podaj� docelowego kraju, na przyk�ad:
```
Shipment provider: LocalPost
Janina Osiwiecka
Kamionka 3/34
Lipsko
25-895
```

Etykiety dla dostawcy `Global` uwzgl�dniaj� kraj adresata, na przyk�ad:
```
Shipment provider: Global
Lea Mathias
9054 Share-wood
Manhattan
90561
USA
```

Nast�pnie, wypisz podsumowanie dla ka�dej przesy�ki (`IParcel`) utworzonej przez dostawc�w w poprzednim kroku.
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

### Wymagania dot. modu�u Shipments

- Dostawcy przesy�ek s� "ci�kimi" obiektami, wi�c zastosuj leniw� inicjalizacj�: dopiero w�wczas, gdy pojawi si� pierwsze zam�wienie do zarejestrowania u danego dostawcy.
- Do wypisania podsumowania przesy�ek zastosuj dostarczony interfejs `ISummaryFormatter` wraz z domy�ln� implementacj� `SummaryFormatter`.
- Dodanie nowych dostawc�w (np. dla poszczeg�lnych kraj�w) nie powinno wymaga� �adnych zmian w "kliencie", czyli w `Program.cs`.
- Dodanie nowych dostawc�w nie powinno te� wi�za� si� z �adnymi zmianami ju� istniej�cych.
- Zmiana struktury bazy stawek podatku VAT nie powinno wymaga� �adnych zmian logiki w obr�bie modu�u Shipments.


## Uwagi
- Obydwa modu�y powinny wypisywa� informacje na konsol� (przyk�ad w pliku `output.txt`).