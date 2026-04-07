# MicrosoftEntraIdDemoApp

## Overview

This demo application demonstrates Single Sign-On (SSO) integration with Microsoft Entra ID (formerly Azure AD). It showcases authentication and user profile retrieval using both React and Angular UI frameworks, highlighting differences in implementation approaches (HTTP interceptors vs. custom hooks).

## Architecture

The project consists of three main components:

### Frontend Applications
- **React UI** — Demonstrates SSO login and user retrieval using custom hooks (`useLogin`) and HTTP client patterns
- **Angular UI** — Provides the same functionality using Angular services and interceptors for comparison

### Backend API
- **API Service** — Acts as a mediator between UI applications and Microsoft Entra ID, handling authentication flow and user data retrieval

## Project Paths

- React: [link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/MicrosoftEntraIdDemoApp/UI/React/microsoft-entra-demo-app)
- Angular: [link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/MicrosoftEntraIdDemoApp/UI/Angular/microsoft-entra-demo-app)
- API: [link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/MicrosoftEntraIdDemoApp/Api/MirsoftEntraDemo.ApiGateway)

## Key Features

- Microsoft Entra ID authentication
- User profile retrieval after login
- Implementation examples in React (hooks-based) and Angular (service-based)
- HTTP client abstraction for API communication
