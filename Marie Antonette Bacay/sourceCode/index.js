const express = require('express');
const app = express();
const User = require('./models/user');
const mongoose = require('mongoose');
const bcrypt = require('bcrypt');

//establish database connection
mongoose.connect('mongodb://localhost:27017/authdemo', { useNewUrlParser: true, useUnifiedTopology: true })
    .then(() => {
        console.log("DATABASE CONNECTION OPEN")
    })
    .catch(err => {
        console.log("DATABASE CONNECTION ERROR")
        console.log(err)
    })

//serve view templates
app.set('view engine', 'ejs');
app.set('views', 'views');
app.use(express.urlencoded({ extended: true }));


//routes
app.get('/', (req, res) => {
    res.render('home')
})

//load register page
app.get('/register', (req, res) => {
    res.render('register');
})


//register user to the database
app.post('/register', async (req, res) => {
  
    const { password, username } = req.body;
    const hash = await bcrypt.hash(password, 12);
    const user = new User({
        username,
        password: hash
    });

    await user.save();
    res.redirect('/');
})

//render login page
app.get('/login', (req, res) => {
    res.render('login');
})

//add new user to the database
app.post('/login', async (req, res) => {
    const { username, password } = req.body;
    const user = await User.findOne({username});
    const validPassword = await bcrypt.compare(password, user.password);

    if (validPassword) 
        res.render('success')
    else 
        res.render('error')
    
})

app.listen(3000, () => {
    console.log("APP RUNNING")
})

