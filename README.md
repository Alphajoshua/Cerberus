Cerberus is the keeper of hell's gate, hound of Hades, a three-headed creature. :wolf::wolf::wolf:
# Cerberus Password Manager :copyright:
Project made in partnership by 
- Contributors : :mortar_board:REA Joris  :mortar_board: REBRAY Malik

## What is Cerberus ?

Cerberus is a password management software written in C#. It aims to help you to manage all your accounts and secure credentials in one simple interface. :lock:

It was first made to satisfy requirements from a university project.
### Project requirements
|  Requirements              |Solution                     |State|
|----------------|-------------------------------|-----------------------------|
|Connection Screen |We implemented a view to allow users to sign up and connect easily through a simple form            |:white_check_mark:         |
|Password Consultation         |You can browse through your list of password, they are by default hidden. When you click on an item, then you can see all fields clearly and modify it as you need. | :white_check_mark:
|Password Management | Not only you are able to add, alter and delete passwords but you can also tag or untag them to sort/filter them easily| :white_check_mark: |
|Use of Xaml Resource | | :white_check_mark: 
|Use of Xaml Styles | | :white_check_mark: |
|Template on List elements | | :white_check_mark: 
| MVVM | The software is written in accordance with the MVVM Design Pattern | :white_check_mark: 
|  Asynchronism | Most of our interactions with the database are made asynchronous. Some operations are still needed to be completed before letting users perform new actions again| :white_check_mark: |
| Graphism | A logo representing Cerberus is used. The design was made to be simple yet elegant. Dark feeling is chosen to respect a night mode use of the software and to cause less pain for the eye | :white_check_mark: 
| Animations | Animations are used to show the passwords management form.| :white_check_mark:
| Clipboard Management | You can quickly and easily copy your password or account ID to the clipboard with our dedicated buttons at the end of a password item line | :white_check_mark:
| Multi-User functionnality | Since anyone has its own account, every user is different. The application can be used accross multiple machines under different account. You only have access to the account registered password. :heavy_exclamation_mark: **You can not share password to another account at the moment.**:heavy_exclamation_mark:| :white_check_mark:
| Multi Factor Authentication | Not implemented yet | :heavy_multiplication_x:
| Passwords Importation | The software doesn't currently support password importation. | :heavy_multiplication_x:
| Passwords Exportation | You can easily export to Excel file (xlsx) or CSV file all your registered password for the account you are logged in | :white_check_mark:
| Automatic Saving | Since all your passwords are saved to a remote Oracle Database (with encryption of course). All changes you make are automatically saved on a distant server | :white_check_mark:
| Automatic Synchronization  | Same as above. Since it's stored on a remote Oracle Database. All changes that you make are registered in the database, they will pass on other users on the same account when they refresh the UI.  | :white_check_mark:

### Extra Features

- Secured and encrypted passwords
- Automatic detection of obsolete passwords
- Custom Images to identify passwords origin
- Integrated Password Generator
