import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ClienteCadastro from './pages/Cliente/ClienteCadastro';
import EventoCadastro from './pages/Evento/EventoCadastro';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/cadastro-cliente" element={<ClienteCadastro/>}/>
        <Route path="/cadastro-evento" element={<EventoCadastro/>}/>    
      </Routes>
    </Router>
  );
}

export default App;
