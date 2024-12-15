# Welcome to Party Helper!
**Party Helper** is a university project developed for the **Advanced Programming Methods** course, serving as a backend application that enables users to create and manage parties, join existing events, offering a streamlined and efficient way to organize and participate in social gatherings.
## Implementation

 1. **Authentication**: JWT (JSON Web Token) is used for securely transferring data between the client and server.
 2.**Models**: Separate models have been created for Members, Party, and Participants.
 3. **CI/CD**: GitHub Actions is used for Continuous Integration and Continuous Deployment.

## How to install Party Helper

 1. First, you need to have Docker installed on your machine.
 2. Open the terminal and run the command: `docker pull ghcr.io/hossamvahid/partyhelper/party-helperbe:latest`.![pull image](https://github.com/user-attachments/assets/34a37ba4-b7e8-4efa-85e8-c351558f089d)
 3. After installing the Docker image, download the `docker-compose` file, open a terminal in the directory where the file is located, and run `docker-compose up`. Once this is done, the installation process will be complete. ![docker compose image](https://github.com/user-attachments/assets/926aa9b4-3681-4f05-a58a-68b921dbaff8)

## Available APIs
After the installation is complete, you can open the container, use an API testing tool, and access the following API URLs.

 - `http://localhost:3278/api/v1/register` - **POST** method  ![Screenshot 2024-12-15 230422](https://github.com/user-attachments/assets/4140f87b-507e-4d83-8ef6-3e975ed1b49b)
 - `http://localhost:3278/api/v1/login` - **POST** method (For logging in with the admin account(use credentials: `admin@admin.com` and `admin`) ![Screenshot 2024-12-15 230513](https://github.com/user-attachments/assets/a2359f7e-e619-402e-9494-10f0ea0326eb)
 - `http://localhost:3278/api/v1/role` - **GET** method for retrieving the role of a member (requires a valid token). 
 - `http://localhost:3278/api/v1/party/create` - **POST** method for creating an party  (requires a valid token). ![Screenshot 2024-12-15 230629](https://github.com/user-attachments/assets/aebc2a70-4821-47c8-9538-f8f14db1037f)
 - `http://localhost:3278/api/v1/party/join/{partyId}` - **POST** method for joining an party  (requires a valid token).
 - `http://localhost:3278/api/v1/party/show/{pageNumber}`-**GET** method for displaying available parties in a paginated format
 - `http://localhost:3278/api/v1/party/delete/{partyId}`-**DELETE** method for deleting a party if you are the owner or an admin (requires a valid token).
 - `http://localhost:3278/api/v1/participant/show/{partyId/{pageNumber}`- **GET** method for displaying members of a party in a paginated format
 - `http://localhost:3278/api/v1/participant/remove/{participantId}`-**DELETE** method for removing a participant from a party, only if you are the party owner, an admin, or the participant who wants to leave (requires a valid token).
 - `http://localhost:3278/api/v1/party/delete`-**DELETE** method for removing parties whose date is in the past
 - `http://localhost:3278/api/v1/member/show/{pageNumber}`-**GET** method for displaying the members for admin in a paginated format (requires a valid token).
 - `http://localhost:3278/api/v1/member/delete/{memberId}`-**DELETE** method for deleting an member for admin  (requires a valid token).
