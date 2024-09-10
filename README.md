1. How did you verify that everything works correctly?
     - Manual testing and code debugging
     - Unit Testin with NUnit
       
2. How long did it take you to complete the task?
     Around 5 - 6 hours
       
3. What else could be done to your solution to make it ready for production?
   Based on the current state of the project, here are several steps that could be taken to make the solution production-ready:
   
- Containerization:
    Create a Dockerfile to containerize the application.
    Develop a docker-compose.yml file for easy deployment and scaling.
- CI/CD Pipeline:
    Implement a CI/CD pipeline using tools like GitHub Actions, GitLab CI, or Azure DevOps.
    Include steps for building, testing, and deploying the application.
- Environment Configuration:
    Ensure all environment-specific configurations (like database connection strings) are stored in environment variables or a secure configuration management system.
- Logging and Monitoring:
    Implement a robust logging system (e.g., Serilog, NLog).
- Set up application monitoring (e.g., Application Insights, Prometheus + Grafana).
- Error Handling and Resilience:
    Implement global exception handling.
    Add retry policies for external service calls (e.g., Polly).
- Security:
    Implement HTTPS redirection and HSTS.
    Add rate limiting to prevent abuse.
    Implement proper authentication and authorization if needed.
- Performance Optimization:
    Implement caching where appropriate (e.g., Redis).
    Consider using a CDN for static assets.
- Database Management:
    Implement database migrations for easier schema updates.
    Set up a separate database for production.
- API Documentation:
    Generate and host API documentation (e.g., Swagger/OpenAPI).
- Load Testing:
Perform load testing to ensure the application can handle expected traffic.
