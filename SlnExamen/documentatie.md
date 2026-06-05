# Documentatie — HelpDesk Examen (OOAD)

**Vak:** Object-Oriented Application Development (OOAD)  
**Academiejaar:** 2025-2026  
**Student:** Alaa Badawi  
**Project:** `SlnExamen/`

---

## 1. Initiële prompt

> Ontwikkel een eenvoudige IT-helpdesktoepassing in C# met een WPF-project en een class library.
>
> Medewerkers melden problemen met **hardware** of **software** via tickets. Een helpdeskmedewerker kan:
> - tickets **raadplegen**;
> - tickets **filteren**;
> - **nieuwe tickets registreren**;
> - tickets **afsluiten**.
>
> Gebruik enkel geziene cursusleerstof. Data wordt opgeslagen in een CSV-bestand (`helpdesk_tickets.csv`), geen databank.

---

## 2. Aanpak

Ik werk in **Cursor** met de geïntegreerde AI-agent. Vaste regels voor elke sessie staan in `AGENTS.md`, zodat de agent steeds dezelfde OOAD-context volgt (cursusleerstof, verboden technieken, architectuur).

Mijn aanpak in grote lijnen:

- **Architectuur:** `CLHelpDesk` voor alle classes en CSV-logica; `WpfHelpDesk` enkel voor de UI.
- **Persistentie:** geen databank — data via `helpdesk_tickets.csv` (cursus hoofdstuk 09).
- **Kleine stappen:** per sessie één duidelijke deeltaak (eerst domein, dan UI, dan testen en bijsturen).
- **Controle:** na elke agent-output zelf nagekeken of er geen verboden technieken in zitten (databinding, LINQ, `var`, …).
- **Testen in Visual Studio:** regelmatig gebouwd en gestart (F5) om te zien of alles werkt zoals verwacht.
- **Documentatie:** dit bestand bijgewerkt na elke belangrijke stap.

---

## 3. Overzicht gebruikte agents

### 3.1 Projectsetup (`AGENTS.md` + documentatie)

**Agenttype:** Agent-modus.

**Vraag**  
Ik vroeg om een agent instruction file en een `documentatie.md` aan te maken in `SlnExamen/`.

**Functionaliteit**  
Startsetup: vaste instructies voor volgende sessies en een documentatiesjabloon.

**Bijsturing**  
De eerste versie van `documentatie.md` was te uitgebreid en leek te veel op een DWD-project. Ik gaf aan dat dit OOAD is en dat de documentatie enkel vijf onderdelen mag bevatten. De agent herschreef de structuur daarop.

---

### 3.2 Regels in `AGENTS.md` (classes, verboden technieken)

**Agenttype:** Agent-modus.

**Vraag**  
Ik gaf de technische randvoorwaarden door: classes verplicht in de class library, CSV i.p.v. databank, enkel cursusleerstof, en een lijst verboden technieken (databinding, DataGrid/GridView/ListView, LINQ, tuples, async/await, `var`, dynamic, user controls, `out`-parameters, …).

**Functionaliteit**  
`AGENTS.md` als vaste referentie voor het hele examenproject.

**Bijsturing**  
Niet nodig.

---

### 3.3 Bestanden en CSV in `AGENTS.md`

**Agenttype:** Agent-modus.

**Vraag**  
Ik verduidelijkte dat de solution uit exact 1 WPF-project en 1 class library bestaat, dat we `helpdesk_tickets.csv` gebruiken, en dat de bestandsregels uit cursus hoofdstuk 09 in `AGENTS.md` moeten komen.

**Functionaliteit**  
Concrete I/O-regels (`File`, `Path`, exception handling) en expliciete uitsluiting van hoofdstuk 13 (databanken).

**Bijsturing**  
Niet nodig.

---

### 3.4 Initiële prompt in documentatie

**Agenttype:** Agent-modus.

**Vraag**  
Ik formuleerde de echte opdrachtomschrijving (helpdesk, tickets, filteren, registreren, afsluiten) en liet die netjes in sectie 1 van de documentatie plaatsen.

**Functionaliteit**  
Duidelijke startprompt als referentie voor het verdere werk.

**Bijsturing**  
Niet nodig.

---

### 3.5 Domeinmodel (`CLHelpDesk`)

**Agenttype:** Agent-modus.

**Vraag**  
Ik vroeg om `TicketPrioriteit`, `Medewerker`, basisklasse `Ticket`, `HardwareTicket` en `SoftwareTicket` aan te maken met courante properties, constructors en methodes. CSV-logica moet in de classes zelf zitten, geen aparte datalayer.

**Functionaliteit**  
Volledige domeinlaag: enum, overerving, `GeefInfo()`, `ToString()`, `LeesAlle()`, `VoegToe()`, `Wijzig()`, …

**Bijsturing**  
`CLHelpDesk` werd op `netstandard2.0` gezet zodat WPF (.NET Framework 4.7.2) de library kan refereren.

---

### 3.6 Classes-regels in `AGENTS.md` (hoofdstuk 10)

**Agenttype:** Agent-modus.

**Vraag**  
Ik liet de regels uit cursus hoofdstuk 10 (classes en properties) toevoegen aan `AGENTS.md`: private variabelen, properties, constructors, object initializer, `ToString()`, pure klassen, geen aparte datalayer.

**Functionaliteit**  
Domein- en codeconventies voor `CLHelpDesk`.

**Bijsturing**  
Niet nodig.

---

### 3.7 WPF-interface (`WpfHelpDesk`)

**Agenttype:** Agent-modus.

**Vraag**  
Ik beschreef de layout: kolom 1 (~30%) met filters (prioriteit, melder, checkbox alleen open) en ListBox; kolom 2 met detail-TextBlock, knop afsluiten, en formulier nieuw ticket (titel, melder, prioriteit, type, toestel, toevoegen).

**Functionaliteit**  
Werkende helpdesk-UI gekoppeld aan `CLHelpDesk`, zonder databinding.

**Bijsturing**  
Niet nodig.

---

### 3.8 WPF- en layoutregels (hoofdstuk 07 + 08)

**Agenttype:** Agent-modus.

**Vraag**  
Ik liet de WPF-regels (hoofdstuk 07) en layout-regels (hoofdstuk 08) toevoegen aan `AGENTS.md`, en vroeg om de bestaande WPF-code aan te passen waar verboden of niet-cursusconforme technieken gebruikt werden.

**Functionaliteit**  
`AGENTS.md` uitgebreid; WPF-code herschreven naar `ComboBoxItem`, `ListBoxItem`, `Label`, `GeefInfo()`, events in code-behind.

**Bijsturing**  
Niet nodig.

---

### 3.9 Documentatie herschrijven

**Agenttype:** Agent-modus.

**Vraag**  
Ik vroeg om `documentatie.md` opnieuw te schrijven in mijn eigen woorden, zonder formuleringen als "de student vraagt".

**Functionaliteit**  
Volledige, chronologische documentatie van het ontwikkelproces tot dan toe.

**Bijsturing**  
Niet nodig.

---

### 3.10 Validatie, commentaar en button states

**Agenttype:** Agent-modus.

**Vraag**  
Ik vroeg om overal validatie in de WPF, commentaar op de code, en duidelijke enabled/disabled-states voor de knoppen (afsluiten en toevoegen).

**Functionaliteit**  
- Validatie via `TextBlock`-meldingen (`txtValidatieTicket`, `txtValidatieNieuw`) i.p.v. `MessageBox`
- Knop **Ticket afsluiten** enkel enabled bij een geselecteerd, open ticket
- Knop **Toevoegen** enkel enabled als alle verplichte velden geldig zijn
- Methodes `ValideerNieuwTicketFormulier()`, `MagTicketAfsluiten()`, `HerstelKnoppen()` in code-behind
- Commentaar en sectie-indeling in `MainWindow.xaml.cs`

**Bijsturing**  
Niet nodig.

---

### 3.11 CSV-pad en lege ListBox (bugfix)

**Agenttype:** Agent-modus.

**Vraag**  
Na testen in Visual Studio was de ListBox leeg. Ik vroeg om het CSV-bestand correct uit te lezen.

**Functionaliteit**  
- CSV-pad aangepast naar `AppDomain.CurrentDomain.BaseDirectory` (naast de `.exe`)
- `helpdesk_tickets.csv` gekoppeld in `WpfHelpDesk.csproj` met `CopyToOutputDirectory: PreserveNewest`
- CSV-parser afgestemd op het werkelijke bestandsformaat (header, quoted regels, datumnotatie)

**Bijsturing**  
Ik testte opnieuw in Visual Studio — daarna verschenen de 12 tickets correct in de ListBox.

---

### 3.12 Eindcontrole en code-opruiming

**Agenttype:** Agent-modus.

**Vraag**  
Ik vroeg om een laatste controle of alle cursusregels gerespecteerd zijn en of de code op sommige plaatsen duidelijker kan.

**Functionaliteit**  
- Geen verboden technieken gevonden (gecontroleerd met grep)
- `Ticket.MaakNieuwTicket()` en `Ticket.ParsePrioriteit()` toegevoegd — minder duplicatie tussen WPF en class library
- `IOException`-afhandeling bij CSV lezen/schrijven (hoofdstuk 09)
- Hulpmethodes in WPF: `HaalUniekeMelders()`, `ToonValidatie()`
- Overbodige usings verwijderd uit `App.xaml.cs`

**Bijsturing**  
Niet nodig. Project compileert succesvol.

---

## 4. Bondige samenvatting

Ik bouwde een IT-helpdesk in C# met twee projecten: **`CLHelpDesk`** (domein + CSV) en **`WpfHelpDesk`** (interface). Tickets zijn hardware- of softwaretickets met prioriteit (Laag, Normaal, Hoog). Een helpdeskmedewerker kan ze raadplegen, filteren op prioriteit/melder/open status, registreren en afsluiten. Data wordt opgeslagen in **`helpdesk_tickets.csv`** — geen databank.

In **`CLHelpDesk`** staan:
- `TicketPrioriteit` (enum)
- `Ticket` (abstracte basisklasse met CSV-methodes: `LeesAlle()`, `VoegToe()`, `Wijzig()`, `MaakNieuwTicket()`, …)
- `HardwareTicket` en `SoftwareTicket` (overerving, `GeefInfo()` override)
- `Medewerker` (aggregatie met `List<Ticket>`)

In **`WpfHelpDesk`**:
- Grid-layout 30%/70% met filters (ComboBox, CheckBox), ListBox voor tickets, detailweergave via `GeefInfo()`
- Formulier voor nieuw ticket met inline validatie
- Events gekoppeld in code-behind; `ComboBoxItem` en `ListBoxItem` overal
- Knoppen met correcte enabled/disabled-logica

Alles volgt de cursus: geen databinding, geen LINQ, geen `var`, geen async/await, expliciete types, `File.Exists()` vóór lezen. `AGENTS.md` bundelt alle regels per cursushoofdstuk. Na testen in Visual Studio werkt de volledige flow: tickets laden, filteren, details bekijken, afsluiten en toevoegen.

---

## 5. Gesprekoverloop

**Sessie 1 — Setup**  
Ik startte met het aanmaken van `AGENTS.md` en `documentatie.md`. De agent leverde een te uitgebreide documentatie. Ik stuurde bij: enkel vijf secties, OOAD-context, geen DWD-inhoud.

**Sessie 2 — Technische regels**  
Ik gaf de lijst verboden technieken en de architectuurregels (classes in library, CSV, geen databank). De agent werkte `AGENTS.md` bij.

**Sessie 3 — Bestanden**  
Ik voegde de regels uit hoofdstuk 09 toe (bestands-I/O, `helpdesk_tickets.csv`). Geen bijsturing nodig.

**Sessie 4 — Initiële prompt**  
Ik formuleerde de echte opdrachtomschrijving (helpdesk, tickets, filteren, registreren, afsluiten) en liet die netjes in de documentatie plaatsen.

**Sessie 5 — Domeinmodel**  
Ik vroeg de enum en klassen aan met CSV-logica in de classes. De library werd gebouwd op `netstandard2.0` en compileerde succesvol.

**Sessie 6 — Classes-regels**  
Ik liet hoofdstuk 10 toevoegen aan `AGENTS.md` (properties, constructors, pure klassen, geen datalayer).

**Sessie 7 — WPF**  
Ik beschreef de volledige UI-layout. De agent bouwde `MainWindow` met filters, ListBox, details en nieuw-ticketformulier, gekoppeld aan `CLHelpDesk`.

**Sessie 8 — WPF/layout cursusregels**  
Ik liet hoofdstuk 07 en 08 toevoegen aan `AGENTS.md` en de WPF-code aanpassen (ComboBoxItem, ListBoxItem, Label, `GeefInfo()`).

**Sessie 9 — Documentatie**  
Ik liet `documentatie.md` herschrijven in mijn eigen woorden, chronologisch en zonder derde-persoonsformuleringen.

**Sessie 10 — Validatie en knoppen**  
Ik vroeg validatie overal in de WPF, commentaar op de code, en correcte enabled/disabled-states voor de knoppen. De agent voegde inline validatie-TextBlocks en `HerstelKnoppen()` toe.

**Sessie 11 — CSV-bugfix**  
Ik testte in Visual Studio: de ListBox was leeg. Ik vroeg om het CSV-bestand correct te lezen. Het pad werd aangepast naar de outputmap en het bestand wordt nu gekopieerd bij build. Na opnieuw testen werkte alles.

**Sessie 12 — Eindcontrole**  
Ik vroeg een laatste check op cursusregels en code-duidelijkheid. Geen verboden technieken gevonden; enkele verbeteringen doorgevoerd (factory-methodes in `Ticket`, IOException-afhandeling, opgeruimde WPF-hulpmethodes).

**Sessie 13 — Documentatie finaliseren**  
Ik liet de documentatie afwerken met alle sessies en een volledige samenvatting van het eindresultaat.
