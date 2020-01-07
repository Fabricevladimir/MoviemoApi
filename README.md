# MoviemoApi (ASP.NET)
.NET Core 3 web api for managing movies in a SQLite database using Entity Framework.

## Functionality Overview
This application manages movies in a database. The source code for a frontend client can be found in [the MoviemoReactClient repo](https://github.com/Fabricevladimir/MoviemoReactClient).

**General functionality:**
* Add, edit, and delete movies and their associated genres
* Supports GET, POST, PUT, and DELETE verbs
* Returns json data

## Genres
### Show genre:
  Returns a single genre
* **URL:**
  /genres/:id
* **Method:**
  `GET`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 200 <br />
    **Content:** `{ id : 1, name : "Action" }`
* **Error Response:**
  * **Code:** 404 NOT FOUND <br />
    **Content:** `{ error: "Genre with id 1 was not found" }`
    
### Show genres:
  Returns json data about all genres
* **URL:**
  /genres
* **Method:**
  `GET`
* **Success Response:**
  * **Code:** 200 <br/>
    **Content:** `[{ id : 1, name : "Action" }, { id : 2, name : "Comedy" }]`
    
### Add genre:
  Returns newly created genre
* **URL:**
  /genres
* **Method:**
  `POST`
* **Success Response:**
  * **Code:** 201 <br />
    **Content:** `{ id : 1, name : "Action" }`
* **Error Response:**
  * **Code:** 400 BAD REQUEST <br />
    **Content:** `{ name: "Invalid name" }`OR

  * **Code:** 409 CONFLICT <br />
    **Content:** `{ error: "Genre with name Action already exists" }`
    
### Edit genre:
  Returns no content
* **URL:**
  /genres/:id
* **Method:**
  `PUT`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 204 <br />
* **Error Response:**
  * **Code:** 400 BAD REQUEST <br />
    **Content:** `{ name: "Invalid name" }`
OR
  * **Code:** 404 NOT FOUND <br />
     **Content:** `{ error: "Genre with id 1 was not found" }`
     

### Delete genre:
  Returns deleted genre
* **URL:**
  /genres/:id
* **Method:**
  `DELETE`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 200 <br />
    **Content:** `{ id : 1, name : "Action" }`
* **Error Response:**
  * **Code:** 404 NOT FOUND <br />
    **Content:** `{ error: "Genre with id 1 was not found" }`
    
## Movies
### Show movie:
  Returns a single movie
* **URL:**
  /movies/:id
* **Method:**
  `GET`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 200 <br />
    **Content:** `{ id : 1, title : "Avengers", genre: "Action", genreId: "1" }`
* **Error Response:**
  * **Code:** 404 NOT FOUND <br />
    **Content:** `{ error: "Movie with id 1 was not found" }`
    
### Show movies:
  Returns json data about all movies
* **URL:**
  /movies
* **Method:**
  `GET`
* **Success Response:**
  * **Code:** 200 <br/>
    **Content:** `[{ id : 1, title : "Avengers", genre: "Action", genreId: "1" }, { id : 2, title : "Jumanji", genre: "Adventure", genreId: "2" }]`
    
### Add movie:
  Returns newly created movie
* **URL:**
  /movies
* **Method:**
  `POST`
* **Success Response:**
  * **Code:** 201 <br />
    **Content:** `{ id : 1, title : "Avengers", genre: "Action", genreId: "1" }`
* **Error Response:**
  * **Code:** 400 BAD REQUEST <br />
    **Content:** `{ title: "Invalid title" }`OR

  * **Code:** 404 NOT FOUND <br />
    **Content:** `{ error: "Genre with id 12 was not found" }`
* **Notes:**
  While creating a genre with an existing name generates an error, multiple movies may have the same title. 
    
### Edit movie:
  Returns no content
* **URL:**
  /movies/:id
* **Method:**
  `PUT`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 204 <br />
* **Error Response:**
  * **Code:** 400 BAD REQUEST <br />
    **Content:** `{ title: "Invalid title" }`
OR
  * **Code:** 404 NOT FOUND <br />
     **Content:** `{ error: "Movie with id 12 was not found" }`
     

### Delete movie:
  Returns deleted movie
* **URL:**
  /movies/:id
* **Method:**
  `DELETE`
* **URL Params:**
  `id=[integer]`
* **Success Response:**
  * **Code:** 200 <br />
    **Content:** `{ id : 1, title : "Avengers", genre: "Action", genreId: "1" }`
* **Error Response:**
  * **Code:** 404 NOT FOUND <br />
    **Content:** `{ error: "Movie with id 12 was not found" }`
