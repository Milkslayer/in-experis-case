# Experis - Wine lottery case

## Oppgave - Vinlotteri app

Lag en liten app basert på casen. Legg koden på Github eller lignende og publiser
løsningen på Azure. Send link til repo og Azure-applikasjonen til intervjuerne senest
24 timer før intervjuet. Forbered en kort presentasjon av koden du har laget til
intervjuet. Ta med egen pc til intervjuet, slik at du kan vise fra på egen maskin.

### Beskrivelse av app

Experis arrangerer vinlotteri hver fredag. I dag gjøres dette manuelt med navn
på et A4 ark med 100 lodd. Du skal lage en app til å digitalisere dette.
Lotteriet fungerer ved at man kjøper et eller flere lodd til 10kr/stk.
Det er som standard 100 lodd til salgs per lotteri. Loddene er nummererte og man
kan selv velge hvilke nummer man vil kjøpe, gitt at de er ledige. X-antall viner
kjøpes inn for ca. 1000 kr. Det trekkes en vinner av en og en vin. Et tall
kan bare trekkes en gang. De dyreste vinene blir loddet ut sist.

### Krav til oppgaven

- Du velger selv hva du ønsker å fokusere på i din løsning. Implementer en eller
  flere deler av vinlotteri appen som beskrevet. Dette kan være kjøp av lodd,
  reservasjon og betaling, eller selve trekningen.
- Det er ikke viktig at alt virker, gjerne mock de delene du ikke rekker å bygge ferdig.
- Fokusere på backend delen av koden med minst 2 APIer, et GET og et POST
- Skriv løsningen i nyeste C# og .Net (Som støttes i Azure).
- Vær forberedt på å kunne presentere strukturen i koden din under intervjuet,
  ta gjerne med
  egen maskin så du viser fram i et miljø du er kjent med selv.
- Publiser løsningen på Azure (man kan opprette gratiskonto)

## MVP Break down
- 1 GET - Get tickets
- 1 POST - Reserve ticket(s)
- Set up services, maybe repository
- Set up storage - EF core with in memory
- Set up Entities - Ticket, Wine, Draw
- State reset
- Azure app service

### If time 
- Lottery draw
- Guards various places
- Sale Report

## Result

**Total time spent**: 2h 13m

### Application can:
- Show list of tickets
- Reserve (sell) tickets
- Draw winners
- Reset

### Possible improvements if more time 
- Persistent data storage (Azure SQL), EF core migrations, store connections strings in Azure Key vault
- Better separation of concern. Lottery service does too much. Separate data fetching logic 
  into repositories, handle lottery session separately.
- Use the information given in the task to bring value. For example, it is stated that each
  ticket is 10kr, then one can assume that some form of a ticket sale report would be valued.
  Additionally some form of report on how many tickets were purchased and how many people participated.
- Payment logic to handle payments. Payment provider integrations
- No present Owner entity for the person who bought the tickets.
- Write unit tests for Domain logic. 
- Write integration tests for service and repository implementations, and api endpoints.
- Authentication and authorization and protect administrative actions.
- Better validation and error handling. Standardized responses.
- Logging, monitoring 
- Ci/cd
- If Saas, then create Documentation and client sdk based on the OpenAPI spec.
- Frontend app
- Security, https, enforce hsts

