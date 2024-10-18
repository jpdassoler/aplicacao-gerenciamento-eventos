import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { fetchConfig, getApiUrl } from '../../config';
import './Home.css';

const Home = () => {
    const [eventos, setEventos] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchEventos = async () => {
            await fetchConfig();
            const apiUrl = getApiUrl();
            try {
                console.log('API URL:', apiUrl);
                const response = await axios.get(`${apiUrl}/Evento`);
                console.log('Response data:', response.data);
                setEventos(response.data);
            } catch (error) {
                console.error('Erro ao buscar eventos:', error.response || error.message || error);
                alert("Erro ao buscar eventos.");
            }
        };
        fetchEventos();
    }, []);

    const handleCardClick = (id) => {
        navigate(`/evento/${id}`);
    };

    const handleCreateEvent = () => {
        navigate("/cadastro-evento");
    };

    return (
        <div className="home-container">
            <button className="create-event-button" onClick={handleCreateEvent}>Criar evento</button>
            <div className="feed">
                {eventos.length > 0 ? (
                    eventos.map((evento) => (
                        <div key={evento.iD_Evento} className="card" onClick={() => handleCardClick(evento.iD_Evento)}>
                            <h3>{evento.nome}</h3>
                            {evento.descricao && <p>{evento.descricao}</p>}                            
                            <div className="card-footer">
                                <span>{new Date(evento.data).toLocaleDateString()}</span>   
                                <span>{evento.endereco.bairro}</span>                                                          
                                <span>{evento.preco_Ingresso ? `R$ ${evento.preco_Ingresso.toFixed(2)}` : 'Gratuito'}</span>
                            </div>
                        </div>
                    ))
                ) : (
                    <p>Nenhum evento encontrado</p>
                )}
            </div>
        </div>
    );
};

export default Home;