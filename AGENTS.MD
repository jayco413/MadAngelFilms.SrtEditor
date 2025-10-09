# AGENTS.md

## Project Specific Notes
- A sample subtitle file is provided: `sample_srt_file.srt`  
- Subtitle naming convention: `Film Name (YYYY).language.srt`  
- `YYYY` = year of release, `language` = two-letter language code  

---

## 🎨 UI Design
- Build desktop experiences with Avalonia 11 and FluentAvalonia UI components
- Prefer `AppWindow` for primary shells and FluentAvalonia visual styles/themes
- Keep the Mad Angel Films color palette and typography (mad_angel_films_colors.txt) as the accent baseline
- Use responsive layouts with `Grid`, `StackPanel`, and `UniformGrid`; honor an 8px spacing rhythm
- Store vector icons (Font Awesome SVGs) in `/Resources/Icons/` and reference them via `avares://`
- Provide light and dark theme support through FluentAvalonia theme resources
- Keep visual hierarchy clear: headers > subheaders > body text; align padding/margins consistently across views

---

## 🧱 UI Naming Conventions
- Use PascalCase for classes, XAML view files, and view-models (`MainWindow.axaml`, `MainWindowViewModel`)
- Use camelCase for local variables and parameters; use `_camelCase` for private fields
- Keep `x:Name` identifiers PascalCase (`SubtitleGrid`, `VideoView`)
- Name event handlers with `On<Control><Event>` when commands are not viable
- Prefer commanding and bindings over code-behind where possible; keep code-behind limited to view-specific orchestration
- Organize Avalonia views under `/Views/` and accompanying view-models under `/ViewModels/`
- Ensure file names match contained types exactly and maintain a consistent folder hierarchy

---

## 🧩 General Naming and Code Rules
- Prefer clarity over brevity  
- Avoid underscores except for private fields  
- Use PascalCase for namespaces, methods, properties, and events  
- Prefix interfaces with “I” (e.g., `IService`)  
- Boolean methods use question prefixes (`IsValid`, `HasAccess`)  
- Properties represent state, not behavior  
- Use async suffix for asynchronous methods  
- File names match contained types  
- One class per file  
- Use bracket-style namespace declarations  
- Avoid Hungarian notation  
- Keep methods focused on a single responsibility  
- Use expression-bodied members when simple  
- Maintain .NET and StyleCop analyzer compliance  

---

## ⚙️ Project Setup
- UI projects host Avalonia views, view-models, and theming only
- Keep domain models and services inside dedicated Core/Controller libraries
- Introduce new class libraries when responsibilities do not fit existing ones
- Reduce coupling and enforce separation of concerns between UI, controllers, and services
- Follow a View ↔ ViewModel ↔ Controller flow; avoid embedding business logic directly in XAML code-behind

---

## 🧠 Architecture Guidelines

### MVVM + Controller Responsibilities
- **Model** – data, DTOs, and external API logic only
- **ViewModel** – state management, commands, and binding glue
- **Controller/Service** – coordination, validation, and business flow
- Keep Avalonia views free of business logic; limit code-behind to UI orchestration
- Views interact with view-models via bindings and commands; view-models call controllers/services
- Use dependency injection or explicit composition when constructing controllers and services
- Keep view-models light and easily testable; reuse DTOs/ViewModels for communication between layers
- Maintain one-way flow: View → ViewModel → Controller/Service → Model → Controller/Service → ViewModel → View

---

### DLL Project Structure
- Each DLL serves a single, clear purpose  
- Never reference UI projects from DLLs  
- Keep dependencies one-directional  
- Use namespaces that mirror DLL structure  
- Name DLLs by domain or responsibility (`.Core`, `.Services`, `.Data`)  
- Keep shared DTOs/constants in `Core` or `Common`  
- Avoid circular dependencies  
- Prefer composition over inheritance  
- DLLs must compile independently from UI  

---

## ⏳ Progress and Feedback
- Surface progress for long-running operations (progress bar, overlay, or status text)
- Update UI via `Dispatcher.UIThread.InvokeAsync` or `IProgress<T>`
- Disable interactive elements while background work is executing and restore them afterward
- Offer cancellation affordances where practical
- Log the start and completion of long-running flows
- Keep the UI thread responsive; never block on synchronous waits
- Reflect actual progress whenever possible (avoid fake animations)

---

## 🛡️ Error Handling and Logging
- Controllers handle exceptions, not Views  
- Display friendly error messages to users  
- Log full exception details internally  
- Use centralized logging from `Core` or `Common`  
- Validate input before performing actions  
- Show non-blocking notifications for recoverable errors  

---

## 🧰 Dependency Injection
- Prefer constructor injection for controllers and services
- Compose dependencies in `Program.cs`, `App.axaml.cs`, or a dedicated bootstrapper
- Avoid global statics or service locators
- Keep object creation out of Avalonia views and code-behind
- Register shared services during startup

---

## 🧪 Validation and ViewModels
- Validate data in the Controller or Service layer  
- Use ViewModels instead of domain models in UI  
- Keep validation logic reusable and centralized  
- Provide inline user feedback for invalid input  
- Avoid binding domain models directly to UI controls  

---

## 🔄 Data Binding and Events
- Prefer Avalonia data binding over manual property setting
- Back collections with `ObservableCollection<T>` and implement `INotifyPropertyChanged` on view-models
- Unsubscribe from events in `OnClosed`/`OnUnloaded` to avoid leaks
- Keep event handlers thin and delegate work to controllers or view-model commands
- Use `DispatcherTimer`/`Dispatcher.UIThread` for UI updates from background threads

---

## 🖼️ Resources and Disposal
- Store reusable assets in `/Resources/` and reference them with `avares://` URIs
- Dispose `IDisposable` objects deterministically (e.g., `LibVLC`, streams, timers)
- Use `using` blocks for file/stream access and unmanaged handles
- Avoid hardcoded file-system paths; rely on configuration or user selection
- Reuse shared resources/styles across views where practical

---

## 🌍 Localization and Accessibility
- Localize strings via `.resx` resources or Avalonia `ResourceDictionary`
- Honor `CultureInfo.CurrentUICulture` when formatting dates/times
- Provide `AutomationProperties.Name`/`HelpText` for interactive controls
- Define unique access keys using `_` in headers (e.g., `_File`)
- Provide keyboard shortcuts (`KeyGesture`) for primary actions
- Ensure contrast ratios meet accessibility guidance
- Preserve full keyboard navigation (Tab/Shift+Tab, arrow keys, access keys)

---

## 🗂️ Tabs, Tab Order & Keyboard Navigation

### Tab Order
- Tab order flows left-to-right, top-to-bottom
- Provide sequential `TabIndex` values with no gaps for focusable elements
- Mark non-interactive controls with `IsTabStop="False"`
- Group related inputs with panels and headers for clarity
- Keep containers earlier in the tab sequence than their children
- Remove hidden/disabled controls from the tab flow
- Expose primary/secondary actions via explicit buttons or key gestures
- Keep focus visuals visible; never suppress focus cues

### TabControl Usage
- Use a single primary `TabControl` where tabs are required
- Limit to 5–7 tabs per view to keep navigation manageable
- Provide access keys in tab headers (`_General`, `_Advanced`)
- Tab order flows from header to the active tab content
- Lazy-load heavy tab content on first activation to keep startup light
- Disable rather than hide unavailable tabs
- Keep tab icons 16×16 and aligned with FluentAvalonia styling
- Use noun-based tab labels (Details, Settings)

### Labels and Inputs
- Pair every input with a descriptive label and access key (`_Name:`)
- Ensure label `Target`/`Tag` associates with the input for accessibility
- Inputs should remain focusable and expose helpful placeholders/tooltips
- Provide accessibility descriptions for all interactive elements

---

## 🧭 Menus and Shortcuts

### General Menu Rules
- Provide a top-level `Menu` within each primary window
- Recommended top-level menus (in order):
  - **_File** – New, Open, Save, Exit
  - **_Edit** – Undo, Redo, Cut, Copy, Paste, Delete
  - **_View** – Toggle Panels, Themes, Refresh
  - **_Tools** – Options, Settings, Preferences
  - **_Help** – Documentation, Check for Updates, _About
- Use unique access keys (underscored characters) per scope
- Keep menu order consistent across windows
- Reuse 16×16 SVG icons from `/Resources/Icons/` where appropriate
- Localize menu headers and commands via resources

### Keyboard Shortcuts
- Standard shortcut assignments:
  - Ctrl+N → New
  - Ctrl+O → Open
  - Ctrl+S → Save
  - Ctrl+Z / Ctrl+Y → Undo / Redo
  - Ctrl+X / Ctrl+C / Ctrl+V → Cut / Copy / Paste
  - Ctrl+Q → Exit
  - F1 → Help
- Use `KeyGesture` bindings for accelerators; display gestures consistently (`Ctrl+S`)
- Avoid overriding system-reserved shortcuts

### Event Handling
- Prefer commands bound to view-model actions; fall back to routed event handlers when necessary
- Validate preconditions before executing actions
- Disable unavailable menu items rather than hiding them
- Handle shared/global actions through reusable controller/view-model methods

### Accessibility and UX
- Ensure menus support keyboard navigation and visible focus states
- Provide tooltips or help text where helpful
- Honor FluentAvalonia color tokens for contrast and hover/pressed states

### Context Menus
- Name context menus `<ControlName>ContextMenu`
- Reuse the same commands/handlers as the main menu where practical
- Keep context menus aligned with available actions; avoid unique-only actions without menu equivalents

### Implementation Notes
- Define menus in XAML when possible for clarity and localization
- Limit hierarchy depth to three levels
- Centralize shared menu styles in a reusable resource dictionary
- Maintain mnemonic uniqueness across localizations

---

## 🚀 Performance and Responsiveness
- Use async operations for I/O and API calls  
- Avoid blocking UI thread; use background tasks  
- Cache immutable or static data  
- Use virtualized controls for large datasets  
- Minimize UI redraw frequency and layout recalculations  

---

## 📏 Code Quality
- Follow `.editorconfig` rules consistently  
- Enable StyleCop and Roslyn analyzers  
- Treat warnings as errors in builds  
- Maintain consistent indentation and brace style  
- XML-document all public members  
- Keep functions small, focused, and testable  

---