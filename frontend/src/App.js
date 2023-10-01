import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [data, setData] = useState(null);
  const [gameId, setGameId] = useState('');
  const [userQuery, setUserQuery] = useState('');
  const [response, setResponse] = useState(null);
  const [gameDataInput, setGameDataInput] = useState('');

  const fetchGameData = async () => {
    try {
      const response = await axios.get(`https://localhost:7085/PokerGame/${gameId}`);
      setData(response.data);
    } catch (error) {
      console.error("Error fetching game data:", error);
    }
  }

  const submitUserQuery = async () => {
    try {
      const response = await axios.post('https://localhost:7085/UserQuery', {
        GameId: gameId || generateGuid(),
        Query: userQuery
      });
      setResponse(response.data);
    } catch (error) {
      console.error("Error submitting user query:", error);
    }
  };

  const submitGameData = async () => {
    try {
      const gameData = JSON.parse(gameDataInput);
      if (!gameData.id) gameData.id = generateGuid();
      if (!gameData.gameId) gameData.gameId = gameData.id;
      if (gameData && gameData.id) {
        setGameId(gameData.id);
      }
      await axios.post('https://localhost:7085/PokerGame', gameData);
      alert("Game data submitted successfully!");
    } catch (error) {
      console.error("Error submitting game data:", error);
    }
  }

  const generateGuid = () => {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0,
        v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  return (
    <div className="App">
      <div className="gameDataContainer">
        <textarea 
          rows="10" 
          cols="50"
          placeholder="Input game data as JSON"
          value={gameDataInput} 
          onChange={e => setGameDataInput(e.target.value)}
        />
        <div className="submitGameDataButton">
          <button onClick={submitGameData}>Submit Game Data</button>
        </div>
      </div>
      
      <div>
        <input 
          type="text" 
          placeholder="Enter game ID" 
          value={gameId} 
          onChange={e => setGameId(e.target.value)}
        />
        <button onClick={fetchGameData}>Fetch Game Data</button>
      </div>
      <div className="fetchedGameData">
  {data ? <pre>{JSON.stringify(data, null, 4)}</pre> : "No game data yet"}
  </div>

      <div>
        <input 
          type="text" 
          placeholder="Enter your query" 
          value={userQuery} 
          onChange={e => setUserQuery(e.target.value)}
        />
        <button onClick={submitUserQuery}>Submit Query</button>
      </div>

      <div style={{ marginTop: '2rem', textAlign: 'center', fontSize: '1.5rem' }}>
        {response ? response : "No response yet"}
      </div>
    </div>
  );
}

export default App;
