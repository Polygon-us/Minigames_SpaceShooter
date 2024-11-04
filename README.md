# PolygonUs_PruebaTecnica
Videojuego top-down como prueba tecnica basado en Twin Bee de NES.

- Un ejecutable del juego se puede descargar desde la pagina de [Releases](https://github.com/acur97/PolygonUs_PruebaTecnica/releases)

# Controles
- Interaccion UI con el Mouse
- Movimiento con las teclas WASD o las Flechas.
- Disparo con clic del Mouse o Ctrl izquierdo.
- Pausa con la tecla Esc.


## Especificaciones técnicas
En este `Readme` se especificaran todos los contenidos

### Requerimientos técnicos
```bash
- Versión  0.0.1
- Tamaño:
    19.7 MB (Build)
    1.39 GB (Proyecto).
- Windows
```

> Unity
> 
La aplicación esta usando actualmente Unity con la versión ```Unity 2021.3.28f1```, con los siguientes paquetes:
```bash
Paquetes base:
    - 2D Pixel Perfect 5.0.3
    - 2D Sprite 1.0.0
    - Burst 1.8.4
    - Core RP Library 12.1.12
    - Custom NUnit 1.0.6
    - Mathematics 1.2.6
    - Searcher 4.9.2
    - Shader Graph 12.1.12
    - Test Framework 1.1.33
    - TextMeshPro 3.0.6
    - Unity UI 1.0.0
    - Universal RP 12.1.12
    - Visual Studio Editor 2.0.18
```
```bash
Otros:
    - LeanTween 2.51
    - febucci Custom Hierarchy 1.2.0
```

## Unity with Nakama Integration

This Unity project is structured with a modular architecture aimed at separating game logic, network functionality, and user interface, inspired by hexagonal architecture principles. The setup leverages Nakama as the backend service for handling multiplayer features and player data.

### Project Structure

The project's main structure is organized under the `/Assets` directory as follows:
```
/Assets
├── /Scripts                      # Main game code
│   ├── /Core                     # Core game logic (no external dependencies)
│   │   ├── /Domain               # Domain classes and models
│   │   │   ├── Player.cs         # Player model
│   │   │   └── Spaceship.cs      # Spaceship model
│   │   ├── /UseCases             # Main application use cases
│   │   │   ├── JoinMatch.cs      # Logic for joining a match
│   │   │   └── SendScore.cs      # Logic for sending score
│   │   └── /Services             # Services for accessing and consuming Nakama
│   │       ├── NakamaService.cs  # Nakama service for connection management
│   │       └── AuthService.cs    # Authentication service
│   │
│   ├── /Infrastructure           # External dependency implementations
│   │   ├── /NakamaAdapters       # Nakama-specific adapters
│   │   │   ├── NakamaClient.cs   # Nakama client
│   │   │   └── NakamaLogger.cs   # Logger for Nakama
│   │   └── /Networking           # General networking logic
│   │       └── NetworkManager.cs  # General network management
│   │
│   ├── /Presentation             # Presentation logic and user interface
│   │   ├── /Controllers          # Controllers interacting with UI and use cases
│   │   │   ├── LoginController.cs # Login controller
│   │   │   └── GameController.cs  # Game controller
│   │   ├── /UI                   # User interface scripts and logic
│   │   │   ├── MainMenuUI.cs     # Main menu UI logic
│   │   │   └── HUD.cs            # In-game UI logic (HUD)
│   │   └── /Views                # Prefabs, UI objects, and view components
│   │       └── LeaderboardView.cs # Leaderboard view
│   │
│   └── /Utils                    # Utilities and helpers
│       ├── Constants.cs          # General project constants
│       └── ExtensionMethods.cs   # Extension methods to simplify code
│
├── /Resources                    # Game resources (textures, sounds)
│   ├── /Prefabs                  # Object and UI prefabs
│   └── /Audio                    # Sound files
├── /Scenes                       # Game scenes
│   ├── MainMenu.unity            # Main menu scene
│   └── Game.unity                # Main game scene
└── /Plugins                      # Nakama SDK and other external plugins
```

#### 1. `/Scripts`
Contains all primary game code divided into distinct layers to enhance code maintainability, testability, and scalability.

- **/Core** - The core layer includes domain classes, core logic, and essential game services that are agnostic of any external dependencies.
  - **/Domain**: Models representing game entities, e.g., `Player.cs` and `Spaceship.cs`.
  - **/UseCases**: Core gameplay logic, e.g., `JoinMatch.cs` for joining matches and `SendScore.cs` for submitting scores.
  - **/Services**: Interfaces with Nakama, handling server-side interactions, e.g., `NakamaService.cs` for server communication and `AuthService.cs` for authentication management.

- **/Infrastructure** - Manages all external integrations and dependencies.
  - **/NakamaAdapters**: Contains classes like `NakamaClient.cs` for Nakama-specific operations and `NakamaLogger.cs` to handle Nakama-related logs.
  - **/Networking**: Manages general network operations via classes like `NetworkManager.cs`.

- **/Presentation** - Handles UI and presentation logic, ensuring separation from the business logic.
  - **/Controllers**: Manages communication between the UI and application logic, e.g., `LoginController.cs` and `GameController.cs`.
  - **/UI**: UI scripts for specific elements such as `MainMenuUI.cs` and `HUD.cs`.
  - **/Views**: Contains prefabs and components for various views, e.g., `LeaderboardView.cs`.

- **/Utils** - Utility functions and constants used throughout the application, like `Constants.cs` for constants and `ExtensionMethods.cs` for reusable helper functions.

#### 2. `/Resources`
Holds game assets like textures, sound files, and prefabs. Organized by type for easy access and management.

- **/Prefabs**: Stores reusable prefabs for UI and game objects.
- **/Audio**: Contains audio files for background music, sound effects, etc.

#### 3. `/Scenes`
Contains the Unity scenes for different parts of the game.

- **MainMenu.unity**: Scene for the main menu interface.
- **Game.unity**: Scene for the core gameplay.

#### 4. `/Plugins`
Houses third-party libraries and SDKs, including the Nakama SDK for Unity.

### Improvements Under Development

We're actively refining the architecture to enhance modularity and scalability. Planned improvements include:

- Expanding **UseCases** for additional game mechanics.
- Enhanced error handling and logging within **Infrastructure/NakamaAdapters**.
- Additional **UI components** for smoother user interaction with Nakama services.

This modular approach ensures that each layer is independent, promoting ease of maintenance and facilitating future expansions.

### Getting Started

1. **Set up Nakama SDK** in `/Plugins` as per [Nakama’s official documentation](https://heroiclabs.com/docs/nakama/unity/).
2. **Load Scenes** - Open `MainMenu.unity` or `Game.unity` in Unity to start exploring the game.
3. **Configuration** - Configure server endpoints in `NakamaService.cs` to connect with your Nakama server.

This structure will help you manage the complexity of multiplayer features and provide a foundation for scalable, maintainable code.
