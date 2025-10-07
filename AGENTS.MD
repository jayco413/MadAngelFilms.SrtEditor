# AGENTS.md

## Project Specific Notes
- A sample subtitle file is provided: `sample_srt_file.srt`  
- Subtitle naming convention: `Film Name (YYYY).language.srt`  
- `YYYY` = year of release, `language` = two-letter language code  

---

## 🎨 UI Design
- Use Font Awesome SVG icons consistently  
- Copy only icons used in the project; remove unused ones  
- Use MaterialSkin.2 for all form styling
- Apply Mad Angel Films color palette and typography (mad_angel_films_colors.txt)
- Use consistent padding, spacing, and color hierarchy  
- Define all icons and images in `/Resources/Icons/`  

---

## 🧱 UI Naming Conventions
- Use PascalCase for classes, forms, and user controls  
- Use camelCase for local variables and parameters  
- Use _camelCase for private fields  
- Use descriptive, meaningful names; avoid abbreviations  
- Match form file names to class names exactly  
- Use consistent naming for controls (`saveButton`, `customerTextBox`)  
- Name event handlers as `ControlName_EventName`  
- Avoid vague names like Button1 or TextBox2  
- Organize controls using `Panel` or `GroupBox`  
- Store all user controls in `/Controls/`  
- Store all forms in `/Forms/`  
- Keep designer and logic code separate  
- Maintain clear and consistent folder hierarchy  

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
- UI projects contain only UI logic  
- Models and business logic belong in separate DLL projects  
- Create new DLLs when responsibilities do not fit existing ones  
- Reduce coupling and enforce separation of concerns  
- Follow Model–View–Controller (MVC) architecture principles  

---

## 🧠 Architecture Guidelines

### MVC Responsibilities
- **Model** – data, DTOs, and external API logic only  
- **View** – UI layout and control display logic only  
- **Controller** – coordination, validation, and business flow  
- No database or API logic inside Forms  
- Views communicate only with Controllers or Services  
- Controllers handle all non-UI logic  
- Use dependency injection for controllers and services  
- Keep Views “dumb” and Controllers “thin”  
- Use DTOs/ViewModels for communication between layers  
- Maintain one-way flow: View → Controller → Model → Controller → View  

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
- All long-running operations must show progress feedback  
- Use `ProgressBar`, overlay spinners, or status bar updates  
- Update progress via `Invoke()` or `IProgress<T>`  
- Disable inputs during operation and re-enable afterward  
- Provide cancel buttons where appropriate  
- Log progress start and completion  
- Keep UI responsive; no blocking calls  
- Reflect real progress, not simulated animation  

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
- Compose dependencies in `Program.cs` or initialization logic  
- Avoid global statics or service locators  
- Keep object creation out of Forms  
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
- Prefer data binding over manual property setting  
- Use `BindingSource` for grids and lists  
- Unsubscribe from events in `Dispose()`  
- Keep event handlers short and delegate to controllers  
- Prevent memory leaks by cleaning up bindings  

---

## 🖼️ Resources and Disposal
- Store all images and icons in `.resx` or `/Resources/`  
- Dispose `IDisposable` objects deterministically  
- Use `using` blocks for `Bitmaps`, `Streams`, and `Graphics`  
- Avoid hardcoded file paths or embedded assets  
- Reuse resources across forms where possible  

---

## 🌍 Localization and Accessibility
- Localize all strings via `.resx`  
- Support multiple cultures via `CurrentUICulture`  
- Assign `AccessibleName` and `AccessibleDescription`  
- Use consistent and unique mnemonics (`&File`, `&Save`)  
- Provide keyboard shortcuts for primary actions  
- Ensure color contrast and legibility  
- Enable full keyboard navigation (Tab, Alt, Arrow keys)  

---

## 🗂️ Tabs, Tab Order & Keyboard Navigation

### Tab Order
- Tab order flows left-to-right, top-to-bottom  
- No duplicate or skipped `TabIndex` values  
- Non-interactive controls must have `TabStop = false`  
- Group inputs in panels or group boxes  
- Containers precede their children in tab sequence  
- Hidden or disabled controls are removed from tab flow  
- Set `AcceptButton` and `CancelButton` for every form  
- `Enter` triggers primary, `Esc` triggers secondary action  
- Keep focus visible; never block tab navigation  

### TabControl Usage
- Use a single `TabControl` named `mainTabControl`  
- Limit to 5–7 tabs per form  
- Each tab must include a mnemonic (`&General`, `&Advanced`)  
- Tab order continues from header to tab content  
- Lazy-load heavy tab content on first activation  
- Disable rather than hide unavailable tabs  
- Keep tab icons 16×16 and style-consistent  
- Tab names should be nouns (e.g., Details, Settings)  

### Labels and Inputs
- Every input requires an associated label with a mnemonic (`&Name:`)  
- Label `TabIndex` precedes input field  
- Label `UseMnemonic = true`, input `TabStop = true`  
- `Alt+<key>` focuses associated control  
- Provide accessibility descriptions for all input controls  

---

## 🧭 Menus and Shortcuts

### General Menu Rules
- Every main form includes a `MenuStrip` named `mainMenuStrip`  
- Required top-level menus (in order):  
  - **&File** – New, Open, Save, Exit  
  - **&Edit** – Undo, Redo, Cut, Copy, Paste, Delete  
  - **&View** – Toggle Panels, Themes, Refresh  
  - **&Tools** – Options, Settings, Preferences  
  - **&Help** – Documentation, Check for Updates, &About  
- All menu items must have mnemonics (`&`)  
- Mnemonics must be unique within scope  
- Menu items must appear in consistent order across forms  
- Use 16×16 SVG icons from `/Resources/Icons/`  
- Align icons to the left of text  
- Localize menu text and mnemonics via `.resx` files  

### Keyboard Shortcuts
- Standard shortcut assignments:  
  - Ctrl+N → New  
  - Ctrl+O → Open  
  - Ctrl+S → Save  
  - Ctrl+Z / Ctrl+Y → Undo / Redo  
  - Ctrl+X / Ctrl+C / Ctrl+V → Cut / Copy / Paste  
  - Ctrl+Q → Exit  
  - F1 → Help  
- Do not override system-reserved shortcuts  
- Shortcut display text must be consistent (`Ctrl+S`)  

### Event Handling
- Menu events must call controller actions, not inline logic  
- Controller methods follow pattern: `Handle<MenuItem>Click()`  
- Validate preconditions before executing menu actions  
- Disable unavailable items instead of hiding them  
- Global actions (Help, About) handled in shared controller  

### Accessibility and UX
- All menus support full keyboard navigation  
- Assign `AccessibleName` and `ToolTipText`  
- Maintain visible focus when navigating  
- Match MaterialSkin theme colors and typography  
- Menu text must meet contrast requirements  
- Include hover highlight and focus feedback  

### Context Menus
- Context menus named `<controlName>_contextMenu`  
- Follow same icons and shortcuts as main menus  
- Reuse same controller event handlers  
- Do not include actions missing from main menu  

### Implementation Notes
- Define menus in Designer, not dynamically  
- Use shared `MenuHelper` for icons, mnemonics, and styles  
- Avoid nesting beyond three levels  
- Store shared templates in `/UI.Common/MenuTemplates/`  
- Preserve mnemonic uniqueness across localizations  

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