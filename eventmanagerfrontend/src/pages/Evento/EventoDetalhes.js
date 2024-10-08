import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './EventoDetalhes.css';

const EventoDetalhes = () => {
    const { id } = useParams();
    const [evento, setEvento] = useState(null);
    const [usuarios, setUsuarios] = useState([]);
    const [abaAtiva, setAbaAtiva] = useState("Sim");

    const navigate = useNavigate();

    useEffect(() => {
        const fetchEvento = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/Evento/${id}`);
                setEvento(response.data);
            } catch (error) {
                console.error('Erro ao buscar evento:', error.response || error.message || error);
                alert("Erro ao buscar evento.");
            }
        };

        fetchEvento();
    }, [id]);

    useEffect(() => {
        const fetchUsuariosComparecimento = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/ClienteEvento/${id}?Ind_Comparecimento=${abaAtiva}`)
            } catch (error) {
                console.error('Erro ao buscar usuários:', error.response || error.message || error);
                alert("Erro ao buscar usuários.");
            }
        };

        fetchUsuariosComparecimento();
    }, [abaAtiva, id]);

    const handleComparecimento = async (status) => {
        try {
            const cliente = JSON.parse(localStorage.getItem('cliente'));

            if (!cliente) {
                alert("Usuário não logado.");
                navigate("/login");
            }

            await axios.post(`${process.env.REACT_APP_API_URL}/ClienteEvento/`, { ID_Evento: id, Usuario: cliente.usuario, Ind_Comparecimento: status });
            alert("Comparecimento registrado.");
        } catch (error) {
            console.error('Erro ao registrar comparecimento:', error.response || error.message || error);
            alert("Erro ao registrar comparecimento.");
        }
    };

    if(!evento) return <div>Carregando...</div>

    return (
        <div className="evento-detalhes-container">
            <h1 className="evento-nome">{evento.nome}</h1>
            <p className="evento-descricao">{evento.descricao || "Sem descrição disponível"}</p>

            <div className="evento-info">
                <p className="evento-info-item"><strong>Data:</strong>{new Date(evento.data).toLocaleDateString()}</p>
                <p className="evento-info-item"><strong>Endereço:</strong>{`${evento.endereco.rua}, ${evento.endereco.numero || ""}, 
                ${evento.endereco.complemento || ""}, ${evento.endereco.bairro}, ${evento.endereco.cidade}, ${evento.endereco.uf} `}</p>
                <p className={`evento-preco ${!evento.preco_Ingresso ? 'gratuito' : ''}`}>
                    {evento.preco_Ingresso ? `R$ ${evento.preco_Ingresso.toFixed(2)}` : 'Gratuito'}
                </p>                
            </div>
            
            {evento.url_Ingresso && (
            <div className="evento-comprar-ingresso">
                <a href={evento.url_Ingresso} className="evento-comprar-link">Comprar Ingressos</a>
            </div>
            )}            

            <div className="comparecimento-container">
                <div className="comparecimento-header">Pessoas confirmadas no evento</div>

                <div className="abas">
                    <div className={`aba ${abaAtiva === 'Sim' ? 'ativa' : ''}`} onClick={() => setAbaAtiva("Sim")}>Sim</div>
                    <div className={`aba ${abaAtiva === 'Nao' ? 'ativa' : ''}`} onClick={() => setAbaAtiva("Nao")}>Não</div>
                    <div className={`aba ${abaAtiva === 'Talvez' ? 'ativa' : ''}`} onClick={() => setAbaAtiva("Talvez")}>Talvez</div>                  
                </div>

                <div className="lista-usuarios">
                    {usuarios.map(usuario => (
                        <div key={usuario.usuario} className="usuario">{usuario.nome}</div>
                    ))}
                </div>

                <div className="icones-comparecimento">
                    <span className="icone" onClick={() => handleComparecimento("Sim")}>✔️</span>
                    <span className="icone" onClick={() => handleComparecimento("Nao")}>❌</span>
                    <span className="icone" onClick={() => handleComparecimento("Talvez")}>🤔</span>
                </div>
            </div>
        </div>
    );
};

export default EventoDetalhes;