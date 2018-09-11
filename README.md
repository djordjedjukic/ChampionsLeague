Koriscena je Hexagonal arhitektura (ports and adapters). 
Za perzistenciju je koriscena InMemory struktura, koju inace koristim za prototyping.
Napisao sam par testova, da pokazem koncept kako bih pisao testove.

Ono sto nisam radio, smatrajuci da bi mozda bilo "overhead", a koje bih inace radio:
1. Druga implementacije perzistencije, konkretno bih radio RavenDB - NoSQL (svakako ova arhitektura podrzava da moze postojati n implementacija perzistencije)
2. Mnogo detaljniji testovi UOW
3. Integracioni testovi koji prakticno mogu da posluze i kao specifikacija (BDD)
4. Swagger za dokumentaciju API
5. Autorizacija
6. Pakovanje aplikacije u docker image

Sto se tice drugih resenja za skalabilnost aplikacije, o tome bih mogao da kazem nesto vise ukoliko budem imao prilike da pricam sa nekim uzivo
