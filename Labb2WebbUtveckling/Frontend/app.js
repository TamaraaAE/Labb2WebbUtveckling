require('dotenv').config();
const express = require('express');
const app = express();
const port = 3000;

// Middleware för att hantera JSON-data och statiska filer
app.use(express.json());
app.use(express.static('.'));

// Servera vår HTML-fil
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/index.html');
});

// Hämta alla blommor från .NET API:et
app.get('/api/blommor', async (req, res) => {
    try {
        const response = await fetch(process.env.API_URL + "/blommor");
        const blommor = await response.json();
        res.json(blommor);
    } catch (error) {
        console.error('Fel vid hämtning av blommor:', error);
        res.status(500).json({ error: 'Kunde inte hämta blommor från API:et' });
    }
});

// Lägg till en ny blomma
app.post('/api/blommor', async (req, res) => {
    try {
        const response = await fetch(process.env.API_URL + "/blommor", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(req.body)
        });

        if (!response.ok) {
            throw new Error('Kunde inte skapa blomma');
        }

        const nyBlomma = await response.json();
        res.status(201).json(nyBlomma);
    } catch (error) {
        console.error('Fel vid skapande av blomma:', error);
        res.status(500).json({ error: 'Kunde inte skapa blomman' });
    }
});

// Starta servern
app.listen(port, () => {
    console.log("Server körs på port " + port);
});