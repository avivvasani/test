const express = require('express');
const fs = require('fs');
const path = require('path');
const app = express();

app.use(express.json());
app.use(express.static(__dirname));

// --- USER DATABASE ---
const allowedUsers = {
    "ashoka_std_01": "pass123",
    "ashoka_std_02": "pass456",
    "admin": "root"
};

// Handle Login
app.post('/login', (req, res) => {
    const { username, password } = req.body;
    if (allowedUsers[username] && allowedUsers[username] === password) {
        res.status(200).send("Login Successful");
    } else {
        res.status(401).send("Invalid Credentials");
    }
});

// Handle Exam Submission
app.post('/submit-exam', (req, res) => {
    const { username, answers } = req.body;
    const filePath = path.join(__dirname, `${username}.json`);

    // Saves the JSON file on the host machine
    fs.writeFile(filePath, JSON.stringify(answers, null, 2), (err) => {
        if (err) {
            console.error(err);
            return res.status(500).send("Error saving data");
        }
        console.log(`Exam results saved for: ${username}`);
        res.status(200).send("Saved Successfully");
    });
});

app.listen(3000, () => {
    console.log("Exam Server is live at http://localhost:3000");
});