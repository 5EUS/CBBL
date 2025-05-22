# CBBL – Chess BitBoard Library

**CBBL** (Chess BitBoard Library) is a lightweight, high-performance C# library providing core tools for representing, manipulating, and querying chess board states using 64-bit (or higher) bitboards. Perfect for engine developers seeking speed and clarity.

---

## Features

- **Efficient Bitboard Operations**  
  - Set, clear, toggle, and test bits  
  - Pop count, least/most significant bit indices  
- **Precomputed Magic Bitboards**  
  - Fast sliding-piece (rook, bishop, queen) move generation  
- **Piece Move Generators**  
  - Pawn, knight, king pseudo-legal moves  
- **Board Utilities**  
  - Rotate, mirror, shift bitboards for attack masks  
- **Debug & Visualization**  
  - ASCII and coordinate-labelled bitboard printers  
- **Extensible Architecture**  
  - Plug in custom move generators or evaluation helpers

---

## Installation

1. **Via NuGet**  
   ```bash
   dotnet add package CBBL
   ```

### *From source* 
    ```bash
    git clone https://github.com/5EUS/CBBL.git
    cd CBBL
    dotnet build
    ```

## Quick Start
    ```csharp
    using CBBL.src.Implementation;
    using CBBL.src.Util;

    var context = ServiceLoader.Instance;
    var board = new CBBLBoard();
    context.Init(board);
    ```
---

# License
    MIT License © 2025 5EUS
