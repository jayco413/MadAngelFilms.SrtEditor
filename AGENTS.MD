# AGENTS.md

## Project Specific Notes
- a sample srt file is provided: sample_srt_file.srt
- naming convention for srt files is Film Name (YYYY).language.srt where YYYY is the year of release, and
- language is the two letter language code.

## 🎨 For UI Design
- Ensure gratuitous use of Font Awesome SVG icons  
- Copy only icons which are needed to the project from the solution folder  
- Remove any icons which are no longer needed from the project  
- Use MaterialSkin.2 for styling  
- Use Mad Angel Films color scheme  

---

## 🧱 UI Project Naming Conventions
- Use PascalCase for form class names  
- Use PascalCase for user control class names  
- Use descriptive, meaningful names for controls  
- Use camelCase for control variable names  
- Avoid vague names like Button1 or TextBox2  
- Group related controls with consistent naming  
- Use full descriptive terms, not abbreviations  
- Use underscores only to separate logical parts if needed  
- Keep names short but readable  
- Align control names with displayed purpose or text  
- Use Panel or GroupBox to organize related controls  
- Keep naming consistent across all forms  
- Use “MainForm” or “AppForm” for main window  
- Name dialog forms ending with “Dialog”  
- Name container forms ending with “Form”  
- Use action-based names for buttons (e.g., Save, Cancel)  
- Use data-based names for labels and text boxes  
- Place form files in `/Forms/` directory  
- Place user controls in `/Controls/` directory  
- Match form file names to class names exactly  
- Use namespaces that reflect UI structure and purpose  
- Keep event handler names descriptive and consistent  
- Use format `ControlName_EventName` for handlers  
- Avoid duplicating logic across forms; centralize where possible  
- Maintain clear layout and folder hierarchy in project  
- Keep designer-generated files separate from logic files  
- Name icons, images, and resources descriptively  
- Store images in a dedicated `/Resources/` or `/Assets/` folder  
- Follow the same casing rules across entire UI layer  

---

## 🧩 General Naming Conventions
- Prefer clarity over brevity  
- Maintain consistent naming across entire project  
- Avoid underscores except private fields  
- Avoid abbreviations unless universally recognized  
- Use PascalCase for namespaces  
- Use PascalCase for classes and structs  
- Use PascalCase with “I” prefix for interfaces  
- Use PascalCase for methods  
- Use PascalCase for properties  
- Use PascalCase for events  
- Use PascalCase for enums and members  
- Use camelCase for local variables  
- Use camelCase for parameters  
- Use _camelCase for private fields  
- Use PascalCase for constants  
- Classes should be nouns or noun phrases  
- Methods should be verbs or actions  
- Boolean methods use question prefixes  
- Properties represent data, not actions  
- Interfaces describe capability or role  
- Enums should be singular type names  
- Parameters should be descriptive and clear  
- Generics use T plus descriptive name  
- File names match contained type  
- Organize folders by domain, not type  
- Async methods use “Async” suffix  
- Do not use Hungarian notation  
- Prefer expression-bodied members when simple  
- Use descriptive names for tests  
- Keep naming consistent with .NET guidelines  
- All files contain only one C# class  
- All classes are namespaced in correct folders  
- Use bracket-style namespace declarations  
- Enforce camelCase consistently throughout codebase  

---

## ⚙️ Project Setup
- Only UI code is allowed in UI projects  
- All model code must be contained in separate DLL projects  
- If an appropriate DLL project does not exist, create a new one  
- Reduce coupling as much as possible  
- Enforce good Model-View-Controller architecture  

---

## 🧠 Architecture Guidelines

### MVC Layer Responsibilities
- **Model** handles data, DTOs, and API logic only  
- **View** handles layout, controls, and display logic only  
- **Controller** manages flow, validation, and coordination  
- Never place data access or logic directly in Forms  
- Views communicate with Controllers or Services only  
- Controllers handle business logic and application rules  
- Use dependency injection for Controllers or Services  
- Keep Views “dumb” — display data, don’t process it  
- Use DTOs or ViewModels for structured communication  
- Each Form should have one primary controller  
- Centralize cross-form logic in shared services  
- Maintain one-way flow: View → Controller → Model → Controller → View  

---

### DLL Project Structure
- Each DLL project serves one clear purpose  
- Never reference UI projects from DLLs  
- DLL dependencies must always be one-directional  
- Use namespaces that mirror the DLL’s structure  
- Name DLLs by domain or responsibility  
- Keep internal classes hidden using `internal`  
- Shared DTOs and constants go in `Core` or `Common`  
- Avoid circular dependencies  
- Prefer composition over inheritance across DLLs  
- Each DLL must compile independently from UI code  
- Use consistent naming and casing across all libraries  

---

## ⏳ Progress and Feedback
- All long-running operations must display progress  
- Use `ProgressBar` or overlay spinner for feedback  
- Always update progress via `Invoke()` or `IProgress<T>`  
- Disable inputs during operation; re-enable on completion  
- Provide cancel button where possible  
- Log start, progress, and completion to status area  
- Keep UI responsive during background tasks  
- Reflect actual operation stages, not fake progress  

---

## 🛡️ Error Handling and Logging
- Controllers handle exceptions, not Views  
- Show friendly error dialogs for user-facing issues  
- Log full exception details internally  
- Use centralized logging utilities in `Common` or `Core`  
- Avoid silent failures or swallowed exceptions  
- Validate inputs before performing operations  
- Display clear status updates for recoverable errors  

---

## 🧰 Dependency Injection
- Prefer constructor injection for Controllers and Services  
- Compose dependencies in `Program.cs` or startup logic  
- Avoid global statics or service locators  
- Keep object creation out of Forms  
- Register all shared services at startup  

---

## 🧪 Validation and ViewModels
- Validate data in Controller or Service layer  
- Use ViewModels instead of domain models in UI  
- Keep validation logic centralized and reusable  
- Provide user feedback inline where possible  
- Avoid direct model binding in Forms  

---

## 🔄 Data Binding and Events
- Prefer data binding over manual property updates  
- Unsubscribe from events on Form disposal  
- Use `BindingSource` for grids and lists  
- Keep event handlers short; delegate logic to Controllers  
- Avoid memory leaks by releasing handlers properly  

---

## 🖼️ Resources and Disposal
- Load images and icons from `.resx` or `/Resources/`  
- Dispose `IDisposable` objects deterministically  
- Use `using` blocks for Bitmaps, Streams, and Graphics  
- No hardcoded file paths or embedded assets in code  
- Reuse image resources; avoid duplication  

---

## 🌍 Localization and Accessibility
- All text strings stored in `.resx` files  
- Support multiple cultures with `CurrentUICulture`  
- Assign `AccessibleName` and `TabIndex` to controls  
- Provide keyboard shortcuts for primary actions  
- Ensure high-contrast and readable color use  

---

## 🚀 Performance and Responsiveness
- Use async methods for I/O or network tasks  
- Avoid blocking the UI thread  
- Use `Task.Run` for heavy computation  
- Cache immutable data to improve load times  
- Minimize unnecessary UI redraws or layout recalculations  
- Use virtualized controls for large datasets  

---

## 📏 Code Quality Enforcement
- Adhere to `.editorconfig` formatting rules  
- Enable Roslyn analyzers and StyleCop in the solution  
- Treat all warnings as errors in CI builds  
- Enforce consistent indentation, spacing, and braces  
- Require XML documentation for public APIs  
- Keep methods small and focused on one responsibility  

---

## 🧭 UI Consistency Checklist
- Use consistent margins and paddings  
- Align labels and controls on a clear grid  
- Keep primary actions aligned to bottom-right  
- Use consistent button order: OK → Cancel → Help  
- Maintain consistent font sizes and weights  
- Use shared color tokens for states (success, warning, error)  
- Ensure icons, spacing, and borders match MaterialSkin theme  

---