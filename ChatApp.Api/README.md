# ChatApp.Api

Chat App Api using Kestrel as the web server. To run the app you need CA or .pem.

Use [mkcert](https://github.com/FiloSottile/mkcert) to create local CA or .pem before running the server. Then put the `*.pem` and `*-key.pem` outside the project folder

```
ChatApp
	- ChatApp.Api
		- Program.cs
		- ...
	- *.pem
	- *-key.pem
```
