# EPSUzduotis

Pastebėjimai:
Programa automatiškai priskiria ip adresą, dėl to gali būti, kad priskirs ne tą. Eilutėje

IPAddress ipAddress = ipHostInfo.AddressList[1];

reikia patikrinti ar ipHostInfo.AddressList[x] atitinka jūsų ip adresą. Jei ne, peržiūrėti sąrašą ir įvesti teisinga reikšmę (x) arba vietoj jo įrašyti 127.0.0.1.
