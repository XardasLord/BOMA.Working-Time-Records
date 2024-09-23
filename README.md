# Salary Management System

This project is an open-source web application designed to help organizations manage employee salaries efficiently. It is built using the .NET API for the backend and Angular for the frontend. Both frameworks are regularly updated to ensure that the system leverages the latest features and security improvements.

The application allows for full customization of salary calculations based on CSV files, which are retrieved by the API at configurable intervals via a CRON job (default is every 5 minutes). The application includes several dashboards and views for users to visualize and manage salary data, employee absences, work summaries, and more.

## Project Goals

The main goal of this project is to offer a flexible and scalable system for managing employee salaries, with a focus on:
- **Customizable CSV import**: Data is ingested through CSV files, enabling organizations to configure the structure and content of the salary data.
- **Real-time updates**: The API retrieves and processes the CSV files at regular intervals (configurable via CRON jobs), ensuring that salary data is always up-to-date.
- **User-friendly dashboards**: The system provides several dashboards that offer insights into employee salaries, absences, work summaries, and more, making it easy for HR teams to make informed decisions.

## Features

- **Automated CSV Processing**: Import employee salary data from customizable CSV files that can be scheduled to be processed every 5 minutes or any other preferred interval.
- **Employee Management**: View and manage employee records, including salaries, absences, and work summaries.
- **Detailed Reports**: Generate reports for salary distribution, attendance, and more.
- **Configurable CRON job**: The API retrieves salary data based on a configurable CRON job, ensuring real-time updates and flexibility in scheduling.
- **Modular Design**: Built with .NET API on the backend and Angular on the frontend, allowing for easy modifications and extension of features.
- **Responsive Design**: The frontend is fully responsive and can be used on different devices such as desktops, tablets, and smartphones.

## Installation Guidelines

### Prerequisites
- .NET SDK (latest stable version)
- Node.js (latest stable version)
- Angular CLI
- SQL Server (or another compatible database)

### Database Setup
The application uses SQL Server by default. Ensure that the database is configured and migration scripts are applied. You can adjust the database settings in appsettings.json.


### Deployment
- Backend: The .NET API can be deployed on any cloud platform supporting .NET (e.g., Azure, AWS, etc.).
- Frontend: The Angular application can be hosted on any web server (e.g., Azure, AWS S3, Firebase Hosting, etc.).
- Configure your CI/CD pipeline to ensure automated deployment to your server of choice.

## Contributing
We welcome contributions from the community! Whether you're fixing bugs, adding new features, or improving documentation, your help is appreciated. Please follow our guidelines for contributing:

- Fork the repository.
- Create a new branch with a descriptive name.
- Make your changes and commit them.
- Submit a pull request.

## License
This project is licensed under the MIT License - see the *[LICENSE file]([https://www.markdownguide.org](https://github.com/XardasLord/BOMA.Working-Time-Records/blob/develop/LICENSE) for details.

## Contact
If you have any questions or issues, feel free to open an issue in the repository or reach out via email at <kowalewicz.pawel@gmail.com>
