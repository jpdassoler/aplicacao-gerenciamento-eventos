import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ClienteCadastro from './pages/Cliente/ClienteCadastro';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/cadastro-cliente" element={<ClienteCadastro/>}/>  
      </Routes>
    </Router>
  );
}

export default App;
