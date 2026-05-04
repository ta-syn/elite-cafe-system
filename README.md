# ☕ Cafe Management System

A robust and modern **Cafe Management System** built with **ASP.NET Core 8.0 MVC**. This application provides a complete solution for managing cafe operations, including menu management, order processing, and user authentication.

---

## ✨ Key Features

### 🔐 Authentication & Authorization
- **Secure Login/Registration**: Multi-role support (Admin & Customer).
- **Session Management**: Secure user sessions with timeout handling.
- **Role-based Access**: Specific views and actions for Admins and Customers.

### 📋 Admin Dashboard
- **Menu Management**: Create, Read, Update, and Delete (CRUD) menu items.
- **Order Tracking**: Monitor real-time orders from customers.
- **User Management**: Overview of registered users and their roles.

### 🛒 Customer Experience
- **Interactive Menu**: Browse menu items categorized for easy access.
- **Cart System**: Add/Remove items and manage quantities before checkout.
- **Order History**: Track personal order status and history.

---

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0 (MVC Pattern)
- **Database**: SQLite (Lightweight & Portable)
- **ORM**: Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
- **Containerization**: Docker & Docker Compose

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (Optional, for containerized deployment)
- A code editor like [VS Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

### Local Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/Cafe-Management.git
   cd Cafe-Management/CafeManagement
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```
   The application will be available at `http://localhost:5000` (or the port specified in your console).

4. **Database Initialization**:
   The system automatically creates the `cafe.db` file and seeds initial data (Admin credentials and sample menu) on the first run.

---

## 🐳 Docker Deployment

The project is fully containerized for easy deployment.

1. **Build the Image**:
   ```bash
   docker build -t cafe-management .
   ```

2. **Run the Container**:
   ```bash
   docker run -p 8080:80 cafe-management
   ```

---

## 📂 Project Structure

```text
CafeManagement/
├── Controllers/    # Application logic & Request handling
├── Models/         # Data structures & ViewModels
├── Views/          # UI Components (Razor Pages)
├── Services/       # Business logic layer
├── Data/           # DB Context & Seeding logic
├── wwwroot/        # Static files (CSS, JS, Images)
└── Program.cs      # Application entry & Configuration
```

---

## 🛡️ License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

*Made with ❤️ for efficient cafe operations.*
