Project Manager Web App

Project Manager is a modern, full-stack web application that allows users to manage projects and tasks efficiently. The app features secure authentication, CRUD operations for projects and tasks, and a responsive interface suitable for both desktop and mobile devices.

Features

User Authentication: Secure registration and login using hashed passwords and JWT tokens.

Project Management: Create, read, update, and delete projects.

Task Management: Create, read, update, and delete tasks within projects.

Ownership Validation: Users can only access and modify their own projects and tasks.

Filtering & Sorting: Sort tasks by title or due date, filter by completion status.

Responsive Frontend: Built with modern frontend frameworks for a smooth user experience.

REST API: Backend implemented in .NET with a scalable repository layer and EF Core for database access.

Tech Stack

Frontend: Vite + React + TypeScript

Backend: ASP.NET Core Web API + Entity Framework Core

Database: SQLite (development) / PostgreSQL (production-ready)

Authentication: JWT-based

Deployment: Frontend on Vercel, Backend on Fly.io

Version Control: Git + GitHub

Installation / Running Locally

Clone the repository:

git clone https://github.com/your-username/project-manager.git


Navigate to backend and install dependencies:

cd ProjectManager.Api
dotnet restore


Configure environment variables:

JWT_KEY – Secret key for JWT tokens

DB_CONNECTION_STRING – Database connection string

Run migrations:

dotnet ef database update


Run backend:

dotnet run


Navigate to frontend and install dependencies:

cd ../ProjectManager.Web
npm install


Configure frontend environment variables:

VITE_API_URL=http://localhost:5000/api


Run frontend:

npm run dev


Deployment

Frontend deployed on Vercel

Backend deployed on Fly.io


Contribution

Contributions are welcome! Please open issues or submit pull requests. Follow clean code and repository layer patterns for consistency.

License

This project is licensed under the MIT License.
