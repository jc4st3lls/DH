# Diffie-Hellman + AES for exchange secrets
Xifrar fitxers amb algotitmes de xifratge simetrics, sense intercanvi de claus secretes.

# Descripció
Dins del móm dels algoritmes de xifratge, existeixen dos grans grups, els algoritmes asimetrics, i els algoritmes simetrics. Els primers requereixen dues claus, una per xifrar, i una altre per desxifrar. En canvi, els simetrics utilitzen la mateixa tant per xifrar com per desxifrar. Els primers, són més segurs, pel fet que l'origen, l'actor que xifra la informació, no ha de transmetre la clau de xifratge al destí, per que aquest la desxifri, ja que el propi algoritme estableix els mecanismes necessàris per que aquest pugui calcular la clau per desxifrar la informació. El problema d'aquest algoritmes és el rendiment, que és més notori quan més creix la informació a xifrar, cosa que fa que es dessestimint en processos que requereixen xifratge i i un alt rendiment de transmissió, com poden ser, sistemes de missatgeria instantànea, establiment de sessions segures en servidors d'aplicacions, sistemes blockchain, tradding, etc. Els segons, són més òptims, i també són segurs, però tenent l'inconvenient de que l'actor origen que xifra ha de transmetre la clau de xifratge a l'actor destí per que aquest pugui desxifrar. Aquest fet, és el que fa vulnerables tots els algoritmes de xifratge simètric, ja que en la transmissió del secret (clau) un tercer el podria capturar.

# Diffie-Hellman
Diffie-Hellman és un algoritme dissenyat per l'intercanvi de secrets (claus) de forma segura. Molt resumidament (a internet està molt documenttat), es basa en que es capaç de calcular el mateix secret en origen i destí. Per fer-ho, cada actor (2 o més) ha de disposar d'un parell de claus, una **privada** i una **pública**, que el mateix algoritme proporciona, i, per generar el secret, tant l'origen com el destí, han de disposar de la **clau pública** de l'altre. 
L'algoritme utilitza la clau privada de l'origen i la clau pública del destí per generar un secret. Igualment, l'algoritme utilitza la clau privada del destí i la clau pública de l'origen per generar un secret. La gràcia està, que amdós secrets són iguals. Per tant, acoseguim tenir el mateix secret en origen i destí sense fer cap transmissió d'aquest. **I per tant, solucionem el problema dels algoritmes de xifratge simetric, si utilitzem aquest secret com a clau de xifratge**.

# Solució
La solució que presento està dividida en dues parts. Una llibreria que fa una petita abstracció dels algoritmes de Diffie-Hellman (bàsic) i Aes, per utilitzar-la en una petita utilitat per xifrar fitxers de text o binàris. Ambdues parts estàn desenvolupades en Net Core 3.1 i són funcionals en Mac, Linux i Windows.

# Ús
És pot executar des del VS, o podeu publicar una versió es pecifica per cada OS.

**Mac:**
```
dotnet publish -c Release -r osx.11.0-x64 --self-contained false
```
**Linux**
```
dotnet publish -c Release -r linux-x64 --self-contained false
```
**Windows**
```
dotnet publish -c Release -r win10-x64 --self-contained false
```

Aquí informació sobre identificadors d'entorn d'execució https://docs.microsoft.com/es-es/dotnet/core/rid-catalog

Exemples (l'ordre dels paràmetres és necessàri, utilitzem Bob i Alice com a àlies)

```
Crear parell de claus: Crea una clau privada [alias].private.key i una clau pública [alias].public.key
DHPoc -alias Bob -createKeys

Esborar les claus:
DHPoc -alias Bob -removeKeys

Xifrar fitxer binari:
DHPoc -alias Alice -encrypt -b Diffie-Hellman.png -pub Bob.public.key

Desxifrar fitxer binari:
DHPoc -alias Bob -decrypt -b Diffie-Hellman.png.dhpoc -pub Alice.public.key

Xifrar fitxer de text:
DHPoc -alias Alice -encrypt -t sample.txt -pub Bob.public.key

Desxifrar fitxer de text:
DHPoc -alias Bob -decrypt -t sample.txt.dhpoc -pub Alice.public.key
```

# Conclusió
I per acabar només dir que, és un algoritme molt utilitzat en el passat i en el present (amb algunes de les seves variants, corbes elíptiques), i que el podem trobar amb eines conegudes com Line, Telegram, Signal, la xarxa Tor, nginx i molts d'altres. Jo personalment, el vaig utilitzar en el meu treball de fi de carrega per desenvolupar un sistema de missatgeria segur, en Java 1.4, quan encara anavem amb modems de 64k. Va suposar una MH :-)

Donacions

BTC = bc1q33wmuc0lcwh4krchfmxrt4jak3v628z658f85j

TRX = TRdxjgLv4nr8GYwsdXkUv4ZwCCTvfMqgnS

ETH = 0x3582cf65c158b23bbbaee3e3a3158ce4b5d99978

:_)




