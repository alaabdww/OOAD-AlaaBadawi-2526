# Agent Instructions — HelpDesk Examen

## Taal
Antwoord altijd in het Nederlands.

## Projecttype
C# examenproject voor **Object-Oriented Application Development (OOAD)**.

De solution bestaat uit **exact twee projecten**:
1. **`CLHelpDesk`** — class library (domein, logica, CSV)
2. **`WpfHelpDesk`** — WPF-presentatie

Geen extra projecten, geen console-app, **geen databank**.

## Solution-structuur
```
SlnExamen/
├── AGENTS.md
├── documentatie.md
├── helpdesk_tickets.csv     ← persistentie (geen database)
├── SlnExamen.slnx
├── CLHelpDesk/              ← class library — alle classes
└── WpfHelpDesk/             ← WPF UI
```

Maak geen nieuwe projecten of mappen aan zonder expliciete toestemming.

## Cursusleerstof (toegestaan)

Gebruik **uitsluitend** geziene leerstof uit de C#-cursus (n00b → pro):

| Hoofdstuk | Onderwerp |
|-----------|-----------|
| 01 | Basis |
| 02 | Variabelen |
| 03 | Selecties |
| 04 | Iteraties |
| 05 | Methodes |
| 06 | Collecties |
| 07 | WPF |
| 08 | Layout |
| 09 | Bestanden |
| 10 | Classes |
| 11 | Static en enum |
| 12 | Overerving |
| — | Appendix .NET, Appendix OO |

**Niet gebruiken:** hoofdstuk 13 (Databanken) — dit project werkt met CSV, geen database.

## Kernregels

### Classes (verplicht)
- **Gebruik van classes is verplicht.**
- **Alle classes** (domein, enums, CSV-logica, businessregels) staan in **`CLHelpDesk`**.
- WPF bevat enkel UI; geen domeinlogica in code-behind.

### Persistentie: CSV (geen databank)
- Data wordt opgeslagen in **`helpdesk_tickets.csv`** (en evt. `helpdesk_medewerkers.csv`).
- Geen SQL, geen Entity Framework, geen databankconnecties.
- CSV-lezen, -schrijven en -parsen hoort in **`CLHelpDesk`**, **in de classes zelf**.
- **Geen aparte datalayer, DataContext of repository-klassen.**
- Methodes als `LeesAlle()`, `VoegToe()`, `SchrijfAlle()` staan op de betreffende class (`Ticket`, `Medewerker`, …).
- WPF roept enkel methodes uit de class library aan; geen CSV in code-behind.

## Architectuurregels

### CLHelpDesk (class library)
- Alle classes, enums, interfaces, static helpers.
- CSV-lezen en -schrijven (`helpdesk_tickets.csv`).
- Geen WPF-, XAML- of `System.Windows`-referenties.
- Namespace: `CLHelpDesk`.

### WpfHelpDesk (presentatie, .NET Framework 4.7.2)
- Enkel XAML, code-behind, event handlers.
- Roept `CLHelpDesk` aan; geen directe CSV-toegang.
- Namespace: `WpfHelpDesk`.

### Referentie
- `WpfHelpDesk` heeft een projectreferentie naar `CLHelpDesk`.

---

## Bestanden en CSV (hoofdstuk 09)

### Toegestane namespaces en klassen

**Paden**
- `System.IO.Path` — `Combine()`, `GetFileName()`, `GetFileNameWithoutExtension()`, `GetExtension()`, `GetDirectoryName()`
- `System.Environment` — `GetFolderPath()`, `SpecialFolder` (Desktop, MyDocuments, …)

**Bestanden**
- `System.IO.File` — `ReadAllText()`, `ReadAllLines()`, `WriteAllText()`, `WriteAllLines()`, `AppendAllText()`, `AppendAllLines()`, `Exists()`, `Copy()`, `Delete()`, `Move()`
- `System.IO.FileInfo` — `Name`, `Extension`, `Exists`, `FullName`, `Length`, `Directory`, …

**Mappen**
- `System.IO.Directory` — `CreateDirectory()`, `Delete()`, `Exists()`, `GetFiles()`, `GetDirectories()`
- `System.IO.DirectoryInfo` — `Name`, `FullName`, `Exists`, `Parent`, …

**Streamend (alleen bij zeer grote bestanden — normaal niet nodig voor CSV)**
- `StreamReader` met `File.OpenText()` en `using`
- `StreamWriter` met `File.CreateText()` en `using`

**Dialoogvensters (WPF)**
- `Microsoft.Win32.OpenFileDialog`
- `Microsoft.Win32.SaveFileDialog`
- `System.Windows.MessageBox`

**Niet gebruiken in dit WPF-project (.NET Framework 4.7.2):**
- `OpenFolderDialog` (pas vanaf .NET 8)
- `System.Windows.Forms.FolderBrowserDialog` (tenzij expliciet gevraagd)

### CSV-bestand: `helpdesk_tickets.csv`

**Pad opbouwen (cursuspatroon):**
```csharp
// CSV staat naast de .exe (bin\Debug), gekopieerd vanuit de solution-map
string map = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.Combine(map, "helpdesk_tickets.csv");
```
In `WpfHelpDesk.csproj` moet `helpdesk_tickets.csv` als Content met `CopyToOutputDirectory` opgenomen zijn.

**Inlezen (voorkeur voor CSV):**
```csharp
if (File.Exists(filePath))
{
    string[] regels = File.ReadAllLines(filePath);
    // elke regel parsen met Split() — geen LINQ
}
```

**Schrijven (volledige lijst overschrijven):**
```csharp
List<string> regels = new List<string>();
// ... regels opbouwen uit objecten
File.WriteAllLines(filePath, regels.ToArray());
// of: string[] regelsArray = ...; File.WriteAllLines(filePath, regelsArray);
```

**Regel toevoegen:**
```csharp
File.AppendAllLines(filePath, new string[] { nieuweRegel });
// of File.AppendAllText() met Environment.NewLine
```

**Bestaan controleren vóór lezen:**
```csharp
if (!File.Exists(filePath))
{
    // bestand aanmaken of lege lijst gebruiken — geen try/catch als alternatief
}
```

**CSV-parsing:** gebruik `string.Split()` en `foreach`/`for`; geen LINQ, geen tuples.

---

## Classes en properties (hoofdstuk 10)

### Class-opbouw
Een class heeft vier soorten members:
- **private variabelen** — gegevens (nooit public)
- **constructors** — hoe objecten aangemaakt worden
- **properties** — toestand (state) van het object
- **methodes** — gedrag (behavior) van het object

Alle domeinklassen staan in **`CLHelpDesk`**. WPF gebruikt ze; definieert ze niet.

### Variabelen vs. properties
| Regel | Afspraak |
|-------|----------|
| Variabelen | Altijd **private**; **nooit** public |
| Properties | Meestal **public**; beginnen met **hoofdletter** |
| Backing field | Private variabelen: **_camelCase** met underscore (`_titel`) |
| Naamgeving | Properties PascalCase (`Titel`), variabelen camelCase (`titel`) |

```csharp
// ❌ FOUT — public variabele
public string Titel;

// ✅ GOED — automatische property (geen validatie nodig)
public string Titel { get; set; }

// ✅ GOED — property met validatie via backing field
private int _rating = 3;
public int Rating
{
    get { return _rating; }
    set
    {
        if (value < 1 || value > 5)
        {
            throw new ArgumentOutOfRangeException("rating moet tussen 1 en 5 liggen");
        }
        _rating = value;
    }
}
```

- **Automatische properties** (`{ get; set; }`) als er geen validatie nodig is.
- **Read-only properties** (`{ get; }`) als de waarde enkel gelezen mag worden.
- **Standaardwaarden** op property: `public int Rating { get; set; } = 3;`
- Validatie in setter met **`value`**; backing field met **`_veldnaam`** (geen `field`-keyword in dit project).

### Constructors
- Elke class heeft minstens een **lege constructor** (expliciet of impliciet).
- **Constructor met parameters** voor verplichte startwaarden.
- **Constructor overloading** en **`:this(...)`** om duplicatie te vermijden.
- **Voorkeur:** lege constructor + **object initializer** i.p.v. veel constructors met parameters:

```csharp
// ✅ voorkeur — object initializer
Ticket ticket = new HardwareTicket()
{
    Id = 1,
    Titel = "Kapotte muis",
    Melder = "Jan",
    Prioriteit = TicketPrioriteit.Hoog,
    ApparaatType = "Muis",
    Serienummer = "SN123"
};
```

### Methodes
- Denk na over **public** (buiten class) vs. **private** (enkel intern).
- Bepaal parameters en returntype bewust.
- **`GeefInfo()`** — eigen methode voor detailweergave (meerdere regels tekst).
- **`override ToString()`** — korte weergave in stringcontext (ListBox, `MessageBox`, …).
- Als iets enkel een waarde teruggeeft en als eigenschap aanvoelt → liever **read-only property** dan methode.

```csharp
public override string ToString()
{
    return $"#{Id} — {Titel} ({Prioriteit})";
}

public override string GeefInfo()
{
    return GeefBasisInfo() + $"\nApparaat: {ApparaatType}";
}
```

(`GeefInfo()` is abstract in basisklasse, `override` in afgeleide klassen.)

### Pure klassen (CLHelpDesk)
Class library-klassen moeten **puur** zijn — los van WPF of Console:

| ❌ Verboden in CLHelpDesk | ✅ Wel toegestaan in CLHelpDesk |
|---------------------------|----------------------------------|
| `Console.WriteLine()`, `Console.ReadKey()` | Bestanden lezen/schrijven (CSV) |
| WPF-controls (`Button`, `TextBox`, …) | `List<T>`, `foreach`, eigen classes |
| `BitmapImage`, `Image`, … | `Random()`, `DateTime`, enums |
| Data binding, XAML | Static methodes op classes |

### Associatie, aggregatie, compositie
- Gebruik classes **in elkaars definitie** als ze gerelateerd zijn.
- **Aggregatie:** object kan bestaan zonder het andere (bv. `Medewerker` met `List<Ticket>`).
- **Compositie:** object kan niet bestaan zonder het andere (sterkere band).
- Voorbeeld: `Medewerker.Tickets` is een `List<Ticket>` — aggregatie.

### Gebruik in WPF (hoofdprogramma)
WPF is het **hoofdprogramma**; het gebruikt classes uit `CLHelpDesk`:

```csharp
List<Ticket> tickets = Ticket.LeesAlle();
lbxTickets.Items.Add(ticket);              // roept ticket.ToString() aan
txtDetails.Text = ticket.GeefInfo();       // detailweergave
Ticket.VoegToe(nieuwTicket);
ticket.SluitAf();
Ticket.Wijzig(ticket);
```

Geen businesslogica of CSV-code in `MainWindow.xaml.cs`.

---

## WPF (hoofdstuk 07)

### Toegestane controls
| Control | Gebruik |
|---------|---------|
| `Button` | Acties (`Click`) |
| `CheckBox` | `IsChecked`, `Checked`/`Unchecked` |
| `ComboBox` | Keuzelijst met `ComboBoxItem` |
| `Label` | Bijschrift bij form controls |
| `ListBox` | Lijst met `ListBoxItem` of objecten (`ToString()`) |
| `TextBlock` | Titels, paragrafen, detailweergave |
| `TextBox` | Vrije tekstinvoer (`.Text`, `.Clear()`) |
| `Border` | Rand rond een control |

Shapes (`Ellipse`, `Rectangle`), `Slider`, `Image`, `DatePicker`, `RadioButton`, `MediaElement` — enkel gebruiken indien nodig.

### Verboden WPF-technieken
- **Geen data binding** (`{Binding ...}`, `ItemsSource=...`)
- **Geen** `DataGrid`, `GridView`, `ListView`
- **Geen User Controls**
- **Geen** MVVM, geen `Invoke`
- **Geen** inline events in XAML (`Click="..."`) — koppel in code-behind na `InitializeComponent()`

### Events (code-behind)
```csharp
public MainWindow()
{
    InitializeComponent();
    btnOpslaan.Click += BtnOpslaan_Click;
    cmbFilter.SelectionChanged += CmbFilter_SelectionChanged;
    chkAlleenOpen.Checked += Filter_Changed;
    chkAlleenOpen.Unchecked += Filter_Changed;
    lbxTickets.SelectionChanged += LbxTickets_SelectionChanged;
}

private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
{
    Button btn = (Button)sender;
    // ...
}
```

Toegestane events: `Click`, `SelectionChanged`, `Checked`, `Unchecked`, `Loaded`, `TextChanged`, `ValueChanged` (Slider).

### ComboBox — altijd `ComboBoxItem`
```csharp
ComboBoxItem item = new ComboBoxItem();
item.Content = "Normaal";
cmbPrioriteit.Items.Add(item);

// geselecteerde waarde opvragen
if (cmbPrioriteit.SelectedItem != null)
{
    ComboBoxItem selectie = (ComboBoxItem)cmbPrioriteit.SelectedItem;
    string waarde = selectie.Content.ToString();
}
```

### ListBox — `ListBoxItem` of object met `ToString()`
```csharp
ListBoxItem item = new ListBoxItem();
item.Content = ticket; // roept ticket.ToString() aan
lbxTickets.Items.Add(item);

// selectie opvragen
if (lbxTickets.SelectedItem != null)
{
    ListBoxItem selectie = (ListBoxItem)lbxTickets.SelectedItem;
    Ticket ticket = (Ticket)selectie.Content;
}
```

### CheckBox
```csharp
if (chkAlleenOpen.IsChecked == true) { /* aangevinkt */ }
chkAlleenOpen.IsChecked = true;
```

### TextBox / TextBlock
```csharp
string titel = txtTitel.Text;
txtTitel.Text = string.Empty; // of txtTitel.Clear();
txtDetails.Text = ticket.GeefInfo(); // meerdere regels met Environment.NewLine
```

### TextBlock vs. Label
- **`Label`** — bijschrift bij interactieve controls (`TextBox`, `ComboBox`, `ListBox`, …)
- **`TextBlock`** — titels, paragrafen, detailweergave (niet-interactief)

### MessageBox
- Gebruik `MessageBox.Show()` voor korte meldingen (fout, bevestiging).
- Prefer `TextBlock` voor inline feedback waar mogelijk.

### Tag property
- Optioneel: extra data aan control koppelen via `Tag` (string).

---

## Layout (hoofdstuk 08)

### Panels — voorkeur
| Panel | Gebruik |
|-------|---------|
| **`Grid`** | Hoofdlayout met kolommen/rijen (voorkeur voor complexe layouts) |
| **`StackPanel`** | Verticale/horizontale stapeling (filters, formulieren) |
| **`Border`** | Rand + padding rond een sectie |
| `DockPanel`, `Canvas`, `WrapPanel` | Enkel indien nodig |

**Geen** `Frame`, `Page`, extra `Window`-navigatie tenzij expliciet gevraagd.

### Grid — kolombreedtes
```xml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="3*"/>   <!-- ~30% -->
    <ColumnDefinition Width="7*"/>   <!-- ~70% -->
</Grid.ColumnDefinitions>
```
- Vaste waarde: `Width="80"`
- Fit content: `Width="Auto"`
- Proportioneel: `Width="3*"`, `Height="*"`

### Positionering — altijd expliciet
Specificeer **`VerticalAlignment`** en **`HorizontalAlignment`** op controls (niet op `Stretch` laten staan tenzij bewust):

```xml
<Button Content="Opslaan"
        HorizontalAlignment="Left"
        VerticalAlignment="Top"
        Padding="12,6"
        Margin="0,0,0,10"/>
```

- **Margin** — ruimte buiten de control (links, boven, rechts, onder)
- **Padding** — ruimte binnen de control
- **TextWrapping="Wrap"** — op `TextBlock` voor meerdere regels

### Window
```xml
<Window Title="IT Helpdesk" Height="600" Width="900"
        MinHeight="500" MinWidth="800">
```

---

## Exception handling (hoofdstuk 09)

### Wanneer wel / niet
- **Wel:** echte I/O-problemen (geen schrijfrechten, bestand geblokkeerd, …).
- **Niet:** ontbrekend bestand → gebruik `File.Exists()` / `Directory.Exists()`.
- **Niet:** eigen logica-fouten wegmoffelen met lege catch-blokken.

### Regels
1. Enkel gebruiken indien echt nodig; prefer `if (File.Exists(...))` boven `catch (FileNotFoundException)`.
2. **Zinvolle catch** — log of toon fout; geen lege catch.
3. **Specifieke types** — bv. `IOException`, niet enkel brede `Exception` tenzij als fallback.
4. **Beperkte try-blok** — enkel de I/O-regel(s) in try, verwerking erbuiten.
5. **Geen `out`-parameters** — gebruik geen `TryParse(..., out ...)`; valideer invoer met if/else of eigen methodes.

Voorbeeld (I/O in try, pad en controle erbuiten):
```csharp
string filePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "helpdesk_tickets.csv");

if (!File.Exists(filePath))
{
    lblMessage.Content = "Bestand helpdesk_tickets.csv niet gevonden.";
    return;
}

string[] regels;
try
{
    regels = File.ReadAllLines(filePath);
}
catch (IOException)
{
    lblMessage.Content = "Kan helpdesk_tickets.csv niet lezen.";
    return;
}

// verwerk regels ...
```

---

## Verboden (strikt)

| Categorie | Verboden |
|-----------|----------|
| Persistentie | Databank, SQL, Entity Framework, hoofdstuk 13 |
| WPF data/UI | Data binding, `DataGrid`, `GridView`, `ListView`, User Controls |
| LINQ | `.Where`, `.Select`, `.Any`, … |
| Types & syntax | `var`, `dynamic`, tuples, structs, `out`-parameters, type switches, case guards |
| Async | `async` / `await` |
| Overig | Expando objects, `Invoke` |

Ook verboden: MVVM, dependency injection, NuGet-packages buiten cursus.

---

## Toegestaan (samenvatting OO/WPF)

- **Hoofdstuk 07:** Button, CheckBox, ComboBox, ListBox, TextBox, TextBlock, Label; events in code-behind; ComboBoxItem/ListBoxItem
- **Hoofdstuk 08:** Grid (3*/7*), StackPanel, Border; Margin, Padding, alignment
- **Hoofdstuk 10:** classes, properties, constructors, `override ToString()`, `GeefInfo()`
- **Hoofdstuk 11–12:** static, enum, overerving, abstract, `base`
- **`List<T>`**, arrays, `foreach`, `for`, `while`
- **Expliciete types** overal (geen `var`)
- Geen data binding; geen `ListView`/`DataGrid`/User Controls

## Werkwijze met de agent

- Kleine stappen; één deeltaak per sessie waar mogelijk.
- Volgorde: classes + CSV in `CLHelpDesk` → UI in `WpfHelpDesk` → `documentatie.md` bijwerken.
- Bijsturing: concreet bestand, probleem, verwacht gedrag.

## Build & run
- Open `SlnExamen.slnx` in Visual Studio.
- Startproject: `WpfHelpDesk`.
- Build de solution vóór je klaar bent.
