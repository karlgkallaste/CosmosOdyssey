# Cosmos Odyssey

**Solar System Travel Deals**

---

## Table of Contents

1. [Overview](#overview)
2. [Technologies](#technologies)
3. [Installation](#installation)

---

## Overview

This project is a full-stack web application built with .NET 8 on the backend and Vite + Vue on the frontend. The
application is designed to query and display the latest prices from a data source, leveraging Hangfire for background
processing in the backend. Hangfire periodically fetches and updates the latest prices, ensuring that the frontend
always displays fresh data.
The frontend, built with Vue and powered by Vite, offers a fast and responsive user interface, communicating with the
backend through RESTful APIs.

---

## Technologies

- **Frontend**: Vue.js, Vite
- **Backend**: .NET 8 (ASP.NET Core)
- **Database**: PostgreSQL
- **Other**: Docker, Azure

---

## Installation

### Prerequisites

Make sure you have the following installed:

- **Node.js**: [Download here](https://nodejs.org/)
- **Docker**: [Download here](https://www.docker.com/)
- **.NET SDK**: [Download here](https://dotnet.microsoft.com/download)

### Steps

1. **Clone the repository**:

   ```
   git clone https://github.com/karlgkallaste/CosmosOdyssey.git
   ```
2. **Navigate to folder**:

   ```
   cd CosmosOdyssey
   ```

3. Build the Docker images
   ```
   docker-compose -f docker-compose-build.yml build
   ```
4. Start the Docker containers:
   ```
   docker-compose -f docker-compose-build.yml up
   ```
