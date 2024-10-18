import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import InputMask from 'react-input-mask';
import { getApiUrl } from '../../config';
import './EventoCadastro.css';

const EventoCadastro = () => {
    const [evento, setEvento] = useState({
        nome: '',
        descricao: '',
        data: '',
        banner: '',
        preco_ingresso: '',
        url_ingresso: 'https://',
        endereco: {
            cep: '',
            rua: '',
            numero: '',
            complemento: '',
            bairro: '',
            cidade: '',
            uf: ''
        }
    });

    const navigate = useNavigate();

    //Buscar o endereço via API Correios ao digitar o CEP
    const buscarEnderecoPorCEP = async (cep) => {
        try {
            const response = await axios.get(`https://viacep.com.br/ws/${cep.replace("-", "")}/json/`);
            if(response.data && !response.data.error) {
                setEvento((prevEvento) => ({
                    ...prevEvento,
                    endereco: {
                        ...prevEvento.endereco,
                        rua: response.data.logradouro || "",
                        bairro: response.data.bairro || "",
                        cidade: response.data.localidade || "",
                        uf: response.data.uf || ""
                    }
                }));
            } else {
                setEvento((prevEvento) => ({
                    ...prevEvento,
                    endereco: {
                        ...prevEvento.endereco,
                        rua: "",
                        bairro: "",
                        cidade: "",
                        uf: ""
                    }
                }));
                alert("CEP não encontrado.")
            }
        } catch(error) {            
            console.error('Erro ao buscar endereço:', error.response || error.message || error);
            setEvento((prevEvento) => ({
                ...prevEvento,
                endereco: {
                    ...prevEvento.endereco,
                    rua: "",
                    bairro: "",
                    cidade: "",
                    uf: ""
                }
            }));
            alert("Erro ao buscar o endereço. Verifique o CEP e tente novamente.");
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        if(name === 'nome' && value.length > 50) return;
        if(name === 'descricao' && value.length > 200) return;
        if(name === 'url_ingresso' && value.length > 200) return;

        setEvento((prevEvento) => ({
            ...prevEvento,
            [name]: value
        }));
    };

    const handleEnderecoChange = (e) => {
        const { name, value } = e.target;

        if(name === 'complemento' && value.length > 200) return;

        setEvento((prevEvento) => ({
            ...prevEvento,
            endereco: {
                ...prevEvento.endereco,
                [name]: value
            }
        }));
    };

    const handlePrecoChange = (e) => {
        const value = e.target.value;
        const formattedValue = value
            .replace(/\D/g, '') // Remove qualquer caractere que não seja número
            .replace(/(\d)(\d{2})$/, '$1.$2') // Adiciona o ponto separador de centavos
            .replace(/(?=(\d{3})+(\D))\B/g, ','); // Adiciona as vírgulas como separador de milhar

        setEvento((prevEvento) => ({
            ...prevEvento,
            preco_ingresso: `R$ ${formattedValue}`
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const eventoData = {
            ...evento,
            descricao: evento.descricao === "" ? null : evento.descricao,
            banner: evento.banner === "" ? null : evento.banner,
            preco_ingresso: evento.preco_ingresso === "" ? null : parseFloat(evento.preco_ingresso.replace(/\./g, '').replace(',', '.')),
            url_ingresso: evento.url_ingresso === "https://" ? null : evento.url_ingresso,
            endereco: {
                ...evento.endereco,
                cep: evento.endereco.cep.replace("-", ""),
                numero: evento.endereco.numero === "" ? null : evento.endereco.numero,
                complemento: evento.endereco.complemento === "" ? null : evento.endereco.complemento 
            }
        };

        const apiUrl = getApiUrl();

        try {
            const response = await axios.post(`${apiUrl}/Evento`, eventoData);
            const eventoId = response.data.iD_Evento;
            alert("Evento cadastrado com sucesso!");
            navigate(`/evento/${eventoId}`);
        } catch(error) {
            console.error('Erro ao cadastrar evento:', error.response || error.message || error);
            alert("Erro ao cadastrar evento.");
        }
    };

    const today = new Date().toISOString().split("T")[0];

    return (
        <div className="evento-cadastro-container">
            <h1>Cadastro de Evento</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="nome">Nome do evento:</label>
                <input type="text" id="nome" name="nome" value={evento.nome} onChange={handleChange} maxLength={50} required/>
                <span className="char-counter">{evento.nome.length}/50</span>
                <label htmlFor="descricao">Descrição:</label>
                <textarea id="descricao" name="descricao" value={evento.descricao} onChange={handleChange} maxLength={200}/>
                <span className="char-counter">{evento.descricao.length}/200</span>
                <label htmlFor="data">Data:</label>
                <input type="date" id="data" name="data" value={evento.data} onChange={handleChange} min={today} required/>
                <label htmlFor="banner">URL do banner:</label>
                <input type="text" id="banner" name="banner" value={evento.banner} onChange={handleChange}/>
                <label htmlFor="preco_ingresso">Preço do ingresso:</label>
                <input type="text" id="preco_ingresso" name="preco_ingresso" value={evento.preco_ingresso} onChange={handlePrecoChange}/>
                <label htmlFor="url_ingresso">URL de compra do ingresso:</label>
                <input type="text" id="url_ingresso" name="url_ingresso" value={evento.url_ingresso} onChange={handleChange} maxLength={200}/>
                <h2>Endereço</h2>
                <label htmlFor="cep">CEP:</label>
                <InputMask mask="99999-999" id="cep" name="cep" value={evento.endereco.cep} onChange={handleEnderecoChange} 
                        onBlur={() => buscarEnderecoPorCEP(evento.endereco.cep)} required/>
                <label htmlFor="rua">Rua:</label>
                <input type="text" id="rua" name="rua" value={evento.endereco.rua} onChange={handleEnderecoChange} required readOnly/>     
                <label htmlFor="numero">Número:</label>
                <input type="text" id="numero" name="numero" value={evento.endereco.numero} onChange={handleEnderecoChange}/>    
                <label htmlFor="complemento">Complemento:</label>
                <input type="text" id="complemento" name="complemento" value={evento.endereco.complemento} onChange={handleEnderecoChange} maxLength={200}/> 
                <label htmlFor="bairro">Bairro:</label>
                <input type="text" id="bairro" name="bairro" value={evento.endereco.bairro} onChange={handleEnderecoChange} required readOnly/>
                <label htmlFor="cidade">Cidade:</label>
                <input type="text" id="cidade" name="cidade" value={evento.endereco.cidade} onChange={handleEnderecoChange} required readOnly/>
                <label htmlFor="uf">UF:</label>
                <input type="text" id="uf" name="uf" value={evento.endereco.uf} onChange={handleEnderecoChange} required readOnly/>
                <button type="submit">Cadastrar Evento</button>
            </form>
        </div>
    );
};

export default EventoCadastro;