# House Party Protocol
It's a file wiper program which can get commands remotely and acts in emergency stiuations. 
Program's name inspired by Iron Man 3 movie where Tony Stark activates the "House Party Protocol" when his house is destroyed.
It's written for preventing your private datas captured by police or thieves. 

## Methods and Features

* Program encrypts target files with AES algorithm for 3 times with random key. It uses unique random key for every single file.  
* Program reads an URL for every 60 seconds to get action command.
* Program can show a message or shutdown the computer after the action is done.

## Usage

* You need to have a hosting account which can run php scripts (you can register for a free one)

Change the `$password` section and upload it to your hosting as `.php` file. And upload a blank `command.txt` file with it.

```php
<?php

$password = "utku123";  // Change it

?>

<html>
<head>
<title>Panel</title>
</head>
<body>
<?php 
if (isset($_POST["password"]) && ($_POST["password"]=="$password")) {
?>

<?php
$file = fopen("command.txt","w");
echo fwrite($file,"1");
fclose($file);
?>
<b>Completed</b>

<?php 
}
else
{

if (isset($_POST['password']) || $password == "") {
  print "<p align=\"center\"><font color=\"red\"><b>Incorrect Password</b><br>Please enter the correct password</font></p>";}
  print "<form method=\"post\"><p align=\"center\">Please enter your password for start wiping<br>";
  print "<input name=\"password\" type=\"password\" size=\"25\" maxlength=\"10\"><input value=\"Login\" type=\"submit\"></p></form>";
}

?>

</body>
</html>
```
* Collect your evidence files into one folder. Click "Select Directory" button and select this folder.
* Paste your command.txt's url to url section (http://yourwebsite.com/command.txt)
* Fill the other settings field
* Press "Save Settings" button
* When you click "Connect" button, the program starts to check command.txt file for every 60seconds. If you give the
command, program will start to wipe your target folder.
* When you click "Connect" button the program will be still checking for the command even pc is restarted. If you
want't to cancel the process, click "Disconnect" button. If you want to change settings, click "Disconnect" button first
* You can wipe your folder via "Wipe Them Now" button manually
* For giving the wipe command, open your `bust.php` file (http://yourwebsite.com/bust.php) with your mobile phone's or computer's browser
* Enter your password
* Action Starts

## Warning

While this may be helpful for some, there are significant risks. You could go to jail on obstruction of justice charges 
just for running House Party Protocol, even though you are innocent.

