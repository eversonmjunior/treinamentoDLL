﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Unimake.Business.DFe.Utility;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Unimake.Business.DFe.Xml.NFe;
using Unimake.Security.Platform;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Servicos.NFe;

namespace TreinamentoDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Certificado Digital
        //Caminho Certificado Digital A1
        private static string PathCertificadoDigital { get; set; } = @"C:\Users\Everson\Desktop\Unimake_PV.pfx";

        //Senha de uso do Certificado Digital A1
        private static string SenhaCertificadoDigital { get; set; } = @"12345678";

        //Field para o Certificado Selecionado
        private static X509Certificate2 CertificadoSelecionadoField;

        private List<Det> CriarDet()        //Percorrer todos os produtos
        {
            var dets = new List<Det>();

            for (int i = 0; i < 1; i++)
            {
                dets.Add(new Det
                {
                    NItem = i + 1,
                    Prod = new Prod     //Dados do Produto
                    {
                        CProd = "01042",
                        CEAN = "SEM GTIN",
                        XProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                        NCM = "84714900",
                        CFOP = "5101",
                        UCom = "LU",
                        QCom = 1.00m,
                        VUnCom = 84.9000000000M,
                        VProd = 84.90,
                        CEANTrib = "SEM GTIN",
                        UTrib = "LU",
                        QTrib = 1.00m,
                        VUnTrib = 84.9000000000M,
                        IndTot = SimNao.Sim,
                        XPed = "300474",
                        NItemPed = 1
                    },
                    Imposto = new Imposto
                    {
                        VTotTrib = 12.63,
                        ICMS = new List<ICMS>
                        {
                          new ICMS
                          {
                            ICMSSN101 = new ICMSSN101
                            {
                                Orig = OrigemMercadoria.Nacional,
                                PCredSN = 2.8255,
                                VCredICMSSN = 2.40
                            }
                          }
                        },
                        PIS = new PIS
                        {
                            PISOutr = new PISOutr
                            {
                                CST = "99",
                                VBC = 0.00,
                                PPIS = 0.00,
                                VPIS = 0.00
                            }
                        },
                        COFINS = new COFINS
                        {
                            COFINSOutr = new COFINSOutr
                            {
                                CST = "99",
                                VBC = 0.00,
                                PCOFINS = 0.00,
                                VCOFINS = 0.00
                            }
                        }
                    }
                });
            }
            return dets;
        }

        public static X509Certificate2 CertificadoSelecionado
        {
            get
            {
                if (CertificadoSelecionadoField == null)         //Carregar Certificado Digital
                {
                    CertificadoSelecionadoField = new CertificadoDigital().CarregarCertificadoDigitalA1(PathCertificadoDigital, SenhaCertificadoDigital);
                }
                return CertificadoSelecionadoField;
            }
            private set => throw new Exception("Não é possível atribuir um certificado digital! Somente resgate o valor da propriedade que o certificado é definido automaticamente.");
        }

        #endregion

        //Serviços NFe
        private void bt_consulta_status_Click(object sender, EventArgs e)
        {
            var xml = new ConsStatServ      //Criando objeto XML
            {
                Versao = "4.00",
                CUF = UFBrasil.PR,
                TpAmb = TipoAmbiente.Homologacao
            };

            var configuracao = new Configuracao         //Configuração do XML
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var statusServico = new StatusServico(xml, configuracao);       //Consumindo o serviço
            statusServico.Executar();
            MessageBox.Show(statusServico.Result.CStat + " " + statusServico.Result.XMotivo);
        }

        private void bt_consulta_situacao_NFe_Click(object sender, EventArgs e)
        {
            var xml = new ConsSitNFe
            {
                Versao = "4.00",
                TpAmb = TipoAmbiente.Homologacao,
                ChNFe = "41190206117473000150550010000557241005753008"
            };

            var configuracao = new Configuracao             //Configuração do XML
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var consultaProtocolo = new ConsultaProtocolo(xml, configuracao);       //Consumindo o serviço
            consultaProtocolo.Executar();
            MessageBox.Show(consultaProtocolo.Result.CStat + " - " + consultaProtocolo.Result.XMotivo);
        }

        private void bt_env_nfe_sinc_Click(object sender, EventArgs e)
        {
            var xml = new EnviNFe
            {
                Versao = "4.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,
                NFe = new List<NFe>         //Construindo a Lista NFe
                {
                    new NFe                 //Construindo uma nota NFe
                    {
                        InfNFe = new List<InfNFe>
                        {
                            new InfNFe
                            {
                                Versao = "4.00",
                                Ide = new Ide       //Identificação
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC. DO ESTABELECIMENTO",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57988,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterestadual,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NormalRetrato,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00"
                                },
                                Emit = new Emit     //Emitente
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new EnderEmit       //Endereço Emitente
                                    {
                                        XLgr = "RUA ANTONIO FELIPE",
                                        Nro = "1500",
                                        XBairro = "CENTRO",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        UF = UFBrasil.PR,
                                        CEP = "87704030",
                                        Fone = "04431414900"
                                    },
                                    IE = "9032000301",
                                    IM = "14018",
                                    CNAE = "6202300",
                                    CRT = CRT.SimplesNacional
                                },
                                Dest = new Dest     //Destinatário
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new EnderDest       //Endereço Destinatário
                                    {
                                        XLgr = "AVENIDA DA SAUDADE",
                                        Nro = "1555",
                                        XBairro = "CAMPOS ELISEOS",
                                        CMun = 3543402,
                                        XMun = "RIBEIRAO PRETO",
                                        UF = UFBrasil.SP,
                                        CEP = "14080000",
                                        Fone = "01639611500"
                                    },
                                    IndIEDest = IndicadorIEDestinatario.ContribuinteICMS,
                                    IE = "582614838110",
                                    Email = "janelaorp@janelaorp.com.br"
                                },
                                Det = CriarDet(),       //Percorrer todos os produtos

                                Total = new Total
                                {
                                    ICMSTot = new ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 84.90,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 84.90,
                                        VTotTrib = 12.63
                                    }
                                },
                                Transp = new Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<Vol>
                                    {
                                        new Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new Cobr()
                                {
                                    Fat = new Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<Dup>
                                    {
                                        new Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new Pag
                                {
                                    DetPag = new List<DetPag>
                                    {
                                            new DetPag
                                            {
                                                IndPag = IndicadorPagamento.PagamentoVista,
                                                TPag = MeioPagamento.Dinheiro,
                                                VPag = 84.90
                                            }
                                    }
                                },
                                InfAdic = new InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474; Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    }
                }
            };

            var configuracao = new Configuracao         //Configuração do XML
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new Autorizacao(xml, configuracao);       //Consumindo o serviço
            autorizacao.Executar();

            //Gravar o arquivo do conteúdo retornado em uma pasta qualquer para ter em segurança.
            //Pode-se também gravar na base de dados. Fica a critério de cada um.
            File.WriteAllText(@"c:\testenfe\retorno\nomearquivoretorno.xml", autorizacao.RetornoWSString);

            if (autorizacao.Result.ProtNFe != null)
            {
                switch (autorizacao.Result.ProtNFe.InfProt.CStat)
                {
                    case 100: //Autorizado o uso da NFe
                    case 110: //Uso Denegado
                    case 150: //Autorizado o uso da NF-e, autorização fora de prazo
                    case 205: //NF-e está denegada na base de dados da SEFAZ [nRec:999999999999999]
                    case 301: //Uso Denegado: Irregularidade fiscal do emitente
                    case 302: //Uso Denegado: Irregularidade fiscal do destinatário
                    case 303: //Uso Denegado: Destinatário não habilitado a operar na UF
                        autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");
                        var docProcNFe = autorizacao.NfeProcResult.GerarXML(); //Gerar o Objeto para pegar a string e gravar em banco de dados
                        MessageBox.Show(autorizacao.NfeProcResult.NomeArquivoDistribuicao);
                        break;

                    default:
                        //NF Rejeitada
                        break;
                }
            }
        }

        private void bt_env_nfe_assinc_Click(object sender, EventArgs e)
        {
            var xml = new EnviNFe
            {
                Versao = "4.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Nao,
                NFe = new List<NFe>         //Construindo a Lista NFe
                {
                    new NFe                 //Construindo uma nota NFe
                    {
                        InfNFe = new List<InfNFe>
                        {
                            new InfNFe
                            {
                                Versao = "4.00",
                                Ide = new Ide       //Identificação
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC. DO ESTABELECIMENTO",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57988,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterestadual,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NormalRetrato,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00"
                                },
                                Emit = new Emit     //Emitente
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new EnderEmit       //Endereço Emitente
                                    {
                                        XLgr = "RUA ANTONIO FELIPE",
                                        Nro = "1500",
                                        XBairro = "CENTRO",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        UF = UFBrasil.PR,
                                        CEP = "87704030",
                                        Fone = "04431414900"
                                    },
                                    IE = "9032000301",
                                    IM = "14018",
                                    CNAE = "6202300",
                                    CRT = CRT.SimplesNacional
                                },
                                Dest = new Dest     //Destinatário
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new EnderDest       //Endereço Destinatário
                                    {
                                        XLgr = "AVENIDA DA SAUDADE",
                                        Nro = "1555",
                                        XBairro = "CAMPOS ELISEOS",
                                        CMun = 3543402,
                                        XMun = "RIBEIRAO PRETO",
                                        UF = UFBrasil.SP,
                                        CEP = "14080000",
                                        Fone = "01639611500"
                                    },
                                    IndIEDest = IndicadorIEDestinatario.ContribuinteICMS,
                                    IE = "582614838110",
                                    Email = "janelaorp@janelaorp.com.br"
                                },
                                Det = CriarDet(),       //Percorrer todos os produtos

                                Total = new Total
                                {
                                    ICMSTot = new ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 84.90,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 84.90,
                                        VTotTrib = 12.63
                                    }
                                },
                                Transp = new Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<Vol>
                                    {
                                        new Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new Cobr()
                                {
                                    Fat = new Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<Dup>
                                    {
                                        new Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new Pag
                                {
                                    DetPag = new List<DetPag>
                                    {
                                            new DetPag
                                            {
                                                IndPag = IndicadorPagamento.PagamentoVista,
                                                TPag = MeioPagamento.Dinheiro,
                                                VPag = 84.90
                                            }
                                    }
                                },
                                InfAdic = new InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474; Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    }
                }
            };
            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new Autorizacao(xml, configuracao);       //Consumindo o serviço
            autorizacao.Executar();

            var configSit = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            if (autorizacao.Result != null)
            {
                if (autorizacao.Result.CStat == 103)     //103 -> Lote recebido com sucesso
                {
                    #region Finalizar através da consulta recibo

                    var xmlRec = new ConsReciNFe
                    {
                        Versao = "4.00",
                        TpAmb = TipoAmbiente.Homologacao,
                        NRec = autorizacao.Result.InfRec.NRec
                    };

                    var configRec = new Configuracao
                    {
                        TipoDFe = TipoDFe.NFe,
                        CertificadoDigital = CertificadoSelecionado
                    };

                    var retAutorizacao = new RetAutorizacao(xmlRec, configRec);
                    retAutorizacao.Executar();

                    autorizacao.RetConsReciNFe = retAutorizacao.Result;
                    autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");

                    #endregion
                }
            }

            #region Finalizando a nota fiscal a partir da consulta situação em caso de não obtenção do recibo da NFe enviada
            foreach (var item in xml.NFe)
            {
                var xmlSit = new ConsSitNFe
                {
                    Versao = "4.00",
                    TpAmb = TipoAmbiente.Homologacao,
                    ChNFe = item.InfNFe[0].Chave
                };

                var consultaProtocolo = new ConsultaProtocolo(xmlSit, configSit);
                consultaProtocolo.Executar();

                autorizacao.RetConsSitNFes.Add(consultaProtocolo.Result);
            }

            autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");

            #endregion
        }

        private void bt_env_nfe_assinc_lote_Click(object sender, EventArgs e)
        {
            {
                var xml = new EnviNFe
                {
                    Versao = "4.00",
                    IdLote = "000000000000001",
                    IndSinc = SimNao.Nao,
                    NFe = new List<NFe>         //Construindo a Lista NFe
                    {
                        new NFe                 //Construindo uma nota NFe
                    {
                        InfNFe = new List<InfNFe>
                        {
                            new InfNFe
                            {
                                Versao = "4.00",
                                Ide = new Ide       //Identificação
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC. DO ESTABELECIMENTO",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57988,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterestadual,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NormalRetrato,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00"
                                },
                                Emit = new Emit     //Emitente
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new EnderEmit       //Endereço Emitente
                                    {
                                        XLgr = "RUA ANTONIO FELIPE",
                                        Nro = "1500",
                                        XBairro = "CENTRO",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        UF = UFBrasil.PR,
                                        CEP = "87704030",
                                        Fone = "04431414900"
                                    },
                                    IE = "9032000301",
                                    IM = "14018",
                                    CNAE = "6202300",
                                    CRT = CRT.SimplesNacional
                                },
                                Dest = new Dest     //Destinatário
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new EnderDest       //Endereço Destinatário
                                    {
                                        XLgr = "AVENIDA DA SAUDADE",
                                        Nro = "1555",
                                        XBairro = "CAMPOS ELISEOS",
                                        CMun = 3543402,
                                        XMun = "RIBEIRAO PRETO",
                                        UF = UFBrasil.SP,
                                        CEP = "14080000",
                                        Fone = "01639611500"
                                    },
                                    IndIEDest = IndicadorIEDestinatario.ContribuinteICMS,
                                    IE = "582614838110",
                                    Email = "janelaorp@janelaorp.com.br"
                                },
                                Det = CriarDet(),       //Percorrer todos os produtos

                                Total = new Total
                                {
                                    ICMSTot = new ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 84.90,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 84.90,
                                        VTotTrib = 12.63
                                    }
                                },
                                Transp = new Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<Vol>
                                    {
                                        new Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new Cobr()
                                {
                                    Fat = new Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<Dup>
                                    {
                                        new Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new Pag
                                {
                                    DetPag = new List<DetPag>
                                    {
                                            new DetPag
                                            {
                                                IndPag = IndicadorPagamento.PagamentoVista,
                                                TPag = MeioPagamento.Dinheiro,
                                                VPag = 84.90
                                            }
                                    }
                                },
                                InfAdic = new InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474; Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    },
                    new NFe                 //Duplicando a nota
                    {
                        InfNFe = new List<InfNFe>
                        {
                            new InfNFe
                            {
                                Versao = "4.00",
                                Ide = new Ide       //Identificação
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC. DO ESTABELECIMENTO",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57988,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterestadual,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NormalRetrato,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00"
                                },
                                Emit = new Emit     //Emitente
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new EnderEmit       //Endereço Emitente
                                    {
                                        XLgr = "RUA ANTONIO FELIPE",
                                        Nro = "1500",
                                        XBairro = "CENTRO",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        UF = UFBrasil.PR,
                                        CEP = "87704030",
                                        Fone = "04431414900"
                                    },
                                    IE = "9032000301",
                                    IM = "14018",
                                    CNAE = "6202300",
                                    CRT = CRT.SimplesNacional
                                },
                                Dest = new Dest     //Destinatário
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new EnderDest       //Endereço Destinatário
                                    {
                                        XLgr = "AVENIDA DA SAUDADE",
                                        Nro = "1555",
                                        XBairro = "CAMPOS ELISEOS",
                                        CMun = 3543402,
                                        XMun = "RIBEIRAO PRETO",
                                        UF = UFBrasil.SP,
                                        CEP = "14080000",
                                        Fone = "01639611500"
                                    },
                                    IndIEDest = IndicadorIEDestinatario.ContribuinteICMS,
                                    IE = "582614838110",
                                    Email = "janelaorp@janelaorp.com.br"
                                },
                                Det = CriarDet(),       //Percorrer todos os produtos

                                Total = new Total
                                {
                                    ICMSTot = new ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 84.90,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 84.90,
                                        VTotTrib = 12.63
                                    }
                                },
                                Transp = new Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<Vol>
                                    {
                                        new Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new Cobr()
                                {
                                    Fat = new Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<Dup>
                                    {
                                        new Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new Pag
                                {
                                    DetPag = new List<DetPag>
                                    {
                                            new DetPag
                                            {
                                                IndPag = IndicadorPagamento.PagamentoVista,
                                                TPag = MeioPagamento.Dinheiro,
                                                VPag = 84.90
                                            }
                                    }
                                },
                                InfAdic = new InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474; Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    }
                }
                };
                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.NFe,
                    TipoEmissao = TipoEmissao.Normal,
                    CertificadoDigital = CertificadoSelecionado
                };

                var autorizacao = new Autorizacao(xml, configuracao);       //Consumindo o serviço
                autorizacao.Executar();

                var configSit = new Configuracao
                {
                    TipoDFe = TipoDFe.NFe,
                    CertificadoDigital = CertificadoSelecionado
                };

                if (autorizacao.Result != null)
                {
                    if (autorizacao.Result.CStat == 103)     //103 -> Lote recebido com sucesso
                    {
                        #region Finalizar através da consulta recibo

                        var xmlRec = new ConsReciNFe
                        {
                            Versao = "4.00",
                            TpAmb = TipoAmbiente.Homologacao,
                            NRec = autorizacao.Result.InfRec.NRec
                        };

                        var configRec = new Configuracao
                        {
                            TipoDFe = TipoDFe.NFe,
                            CertificadoDigital = CertificadoSelecionado
                        };

                        var retAutorizacao = new RetAutorizacao(xmlRec, configRec);
                        retAutorizacao.Executar();

                        autorizacao.RetConsReciNFe = retAutorizacao.Result;
                        autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");

                        #endregion
                    }
                }

                #region Finalizando a nota fiscal a partir da consulta situação em caso de não obtenção do recibo da NFe enviada
                foreach (var item in xml.NFe)
                {
                    var xmlSit = new ConsSitNFe
                    {
                        Versao = "4.00",
                        TpAmb = TipoAmbiente.Homologacao,
                        ChNFe = item.InfNFe[0].Chave
                    };

                    var consultaProtocolo = new ConsultaProtocolo(xmlSit, configSit);
                    consultaProtocolo.Executar();

                    autorizacao.RetConsSitNFes.Add(consultaProtocolo.Result);
                }

                autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");

                #endregion
            }
        }

        private void bt_inutilizacao_Click(object sender, EventArgs e)
        {
            var xml = new InutNFe
            {
                Versao = "4.00",
                InfInut = new InutNFeInfInut
                {
                    Ano = "22",
                    CNPJ = "06117473000150",
                    CUF = UFBrasil.PR,
                    Mod = ModeloDFe.NFe,
                    NNFIni = 54569,
                    NNFFin = 54570,
                    Serie = 1,
                    TpAmb = TipoAmbiente.Homologacao,
                    XJust = "Justificativa de inutilização para teste"
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var inutilizacao = new Inutilizacao(xml, configuracao);
            inutilizacao.Executar();

            switch (inutilizacao.Result.InfInut.CStat)
            {
                case 102:   //Inutilização homologada
                    inutilizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");
                    break;
                default:
                    //Tratamentos necessários caso tenha rejeição 
                    break;
            }
        }

        private void bt_cons_cadas_contribuinte_Click(object sender, EventArgs e)
        {
            var xml = new ConsCad
            {
                Versao = "2.00",
                InfCons = new InfCons()
                {
                    CNPJ = "06117473000150",
                    UF = UFBrasil.PR
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var consultaCad = new ConsultaCadastro(xml, configuracao);
            consultaCad.Executar();

            MessageBox.Show(consultaCad.RetornoWSString);
            MessageBox.Show(consultaCad.Result.InfCons.XMotivo);
        }

        private void bt_env_evento_cancel_nfe_Click(object sender, EventArgs e)
        {
            var xml = new EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<Evento>
                {
                    new Evento
                    {
                        Versao = "1.00",
                        InfEvento = new InfEvento(new DetEventoCanc
                        {
                            NProt = "141190000660363",
                            Versao = "1.00",
                            XJust = "Justificativa de teste de cancelamento"
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41201280568835000181580010000010411406004656",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.Cancelamento,
                            NSeqEvento = 1,
                            VerEvento = "1.00",
                            TpAmb = TipoAmbiente.Homologacao
                        }
                    }
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            if (recepcaoEvento.Result.CStat == 128)          //128 -> Lote de evento processado com sucesso
            {
                switch (recepcaoEvento.Result.RetEvento[0].InfEvento.CStat)
                {
                    case 135: //Evento Homologado
                    case 155: //Evento homologado fora do prazo permitido
                        recepcaoEvento.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL");
                        break;

                    default:
                        //Ações necessárias caso seja rejeitada
                        break;
                }
            }
        }

        private void bt_env_nfe_desserializacao_Click(object sender, EventArgs e)
        {
            #region Deserializar XML da NFe sem o lote

            var doc = new XmlDocument();
            doc.Load(@"C:\ProjetoLivesDLL\TreinamentoDLL\Recursos\NFe.xml");

            var xml = new EnviNFe
            {
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,
                Versao = "4.00"
            };

            xml.NFe = new List<NFe>();
            xml.NFe.Add(XMLUtility.Deserializar<NFe>(doc));

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new Autorizacao(xml, configuracao);
            autorizacao.Executar();

            #endregion
        }

        private void bt_env_evento_cce_Click(object sender, EventArgs e)
        {
            var xml = new EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<Evento>
                {
                    new Evento
                    {
                        Versao = "1.00",
                        InfEvento = new InfEvento(new DetEventoCCE
                        {
                            Versao = "1.00",
                            XCorrecao = "CFOP errada, segue CFOP correta. teste."
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41201280568835000181580010000010411406004656",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.Cancelamento,
                            NSeqEvento = 1,
                            VerEvento = "1.00",
                            TpAmb = TipoAmbiente.Homologacao
                        }
                    },
                    new Evento
                    {
                        Versao = "1.00",
                        InfEvento = new InfEvento(new DetEventoCCE
                        {
                            Versao = "1.00",
                            XCorrecao = "Nome do transportador está errado, segue nome correto."
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41191006117473000150550010000579281779843610",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.CartaCorrecao,
                            NSeqEvento = 4,
                            VerEvento = "1.00",
                            TpAmb = TipoAmbiente.Homologacao
                        }
                    }
                }
            };
            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            if (recepcaoEvento.Result.CStat == 128)      //128 -> Lote de evento processado com suceso
            {
                switch (recepcaoEvento.Result.RetEvento[0].InfEvento.CStat)
                {
                    case 135: //Evento homologado vinculado com respectiva NFe
                        recepcaoEvento.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\TreinamentoDLL\Recursos\NFe.xml");
                        break;

                    default:
                        //Ações necessárias
                        break;
                }
            }

        }

        //Fim Serviços NFe

        //Serviços NFCe

        private void bt_consulta_status_nfce_Click(object sender, EventArgs e)
        {
            var xml = new ConsStatServ
            {
                Versao = "4.00",
                CUF = UFBrasil.PR,
                TpAmb = TipoAmbiente.Homologacao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var statusServico = new Unimake.Business.DFe.Servicos.NFCe.StatusServico(xml, configuracao);
            statusServico.Executar();

            MessageBox.Show(statusServico.Result.CStat + " " + statusServico.Result.XMotivo);
        }

        private void bt_consulta_situacao_nfce_Click(object sender, EventArgs e)
        {
            var xml = new ConsSitNFe
            {
                Versao = "4.00",
                TpAmb = TipoAmbiente.Homologacao,
                ChNFe = "41201280568835000181580010000010411406004656"
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var consultaProtocolo = new Unimake.Business.DFe.Servicos.NFCe.ConsultaProtocolo(xml, configuracao);
            consultaProtocolo.Executar();

            MessageBox.Show(consultaProtocolo.Result.CStat + " " + consultaProtocolo.Result.XMotivo);
        }

        private void bt_env_nfce_sinc_Click(object sender, EventArgs e)
        {
            var xml = new EnviNFe
            {
                Versao = "4.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,

                NFe = new List<NFe>
                {
                    new NFe
                    {
                        InfNFe = new List<InfNFe>
                        {
                            new InfNFe
                            {
                                Versao = "4.00",
                                Ide = new Ide
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFCe,
                                    Serie = 1,
                                    NNF = 57980,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterna,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NFCe,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00"
                                },

                                Emit = new Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new EnderEmit
                                    {
                                        XLgr = "RUA ANTONIO FELIPE",
                                        Nro = "1500",
                                        XBairro = "CENTRO",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        UF = UFBrasil.PR,
                                        CEP = "87704030",
                                        Fone = "04431414900"
                                    },
                                    IE = "9032000301",
                                    IM = "14018",
                                    CNAE = "6202300",
                                    CRT = CRT.SimplesNacional
                                },

                                Det = new List<Det>
                                {
                                    new Det
                                    {
                                        NItem = 1,
                                        Prod = new Prod
                                        {
                                            CProd = "01042",
                                            CEAN = "SEM GTIN",
                                            XProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                            NCM = "84714900",
                                            CFOP = "5101",
                                            UCom = "LU",
                                            QCom = 1.00m,
                                            VUnCom = 84.9000000000M,
                                            VProd = 84.90,
                                            CEANTrib = "SEM GTIN",
                                            UTrib = "LU",
                                            QTrib = 1.00m,
                                            VUnTrib = 84.9000000000M,
                                            IndTot = SimNao.Sim,
                                            XPed = "300474",
                                            NItemPed = 1
                                        },

                                        Imposto = new Imposto
                                        {
                                            VTotTrib = 12.63,
                                            ICMS = new List<ICMS>
                                            {
                                                new ICMS
                                                {
                                                    ICMSSN102 = new ICMSSN102
                                                    {
                                                        Orig = OrigemMercadoria.Nacional,
                                                        CSOSN = "102"
                                                    }
                                                }
                                            },

                                            PIS = new PIS
                                            {
                                                PISOutr = new PISOutr
                                                {
                                                    CST = "99",
                                                    VBC = 0.00,
                                                    PPIS = 0.00,
                                                    VPIS = 0.00
                                                }
                                            },

                                            COFINS = new COFINS
                                            {
                                                COFINSOutr = new COFINSOutr
                                                {
                                                    CST = "99",
                                                    VBC = 0.00,
                                                    PCOFINS = 0.00,
                                                    VCOFINS = 0.00
                                                }
                                            }
                                        }
                                    }
                                },
                                Total = new Total
                                {
                                    ICMSTot = new ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 84.90,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 84.90,
                                        VTotTrib = 12.63
                                    }
                                },

                                Transp = new Transp
                                {
                                    ModFrete = ModalidadeFrete.SemOcorrenciaTransporte
                                },
                                Cobr = new Cobr()
                                {
                                    Fat = new Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<Dup>
                                    {
                                        new Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },

                                Pag = new Pag
                                {
                                    DetPag = new List<DetPag>
                                    {
                                        new DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 84.90,
                                        }
                                    }
                                },

                                InfAdic = new InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    }
                },
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado,
                CSC = "HCJBIRTWGCQ3HVQN7DCA0ZY0P2NYT6FVLPJG",
                CSCIDToken = 2
            };

            var autorizacao = new Unimake.Business.DFe.Servicos.NFCe.Autorizacao(xml, configuracao);
            autorizacao.Executar();

            if (autorizacao.Result.ProtNFe != null)
            {
                switch (autorizacao.Result.ProtNFe.InfProt.CStat)
                {
                    case 100: //Autorizado o uso da NFe
                    case 110: //Uso Denegado
                    case 150: //Autorizado o uso da NF-e, autorização fora de prazo
                    case 205: //NF-e está denegada na base de dados da SEFAZ [nRec:999999999999999]
                    case 301: //Uso Denegado: Irregularidade fiscal do emitente
                    case 302: //Uso Denegado: Irregularidade fiscal do destinatário
                    case 303: //Uso Denegado: Destinatário não habilitado a operar na UF
                        autorizacao.GravarXmlDistribuicao(@"c:\testenfe\");
                        var docProcNFe = autorizacao.NfeProcResult.GerarXML(); //Gerar o Objeto para pegar a string e gravar em banco de dados
                        MessageBox.Show(autorizacao.NfeProcResult.NomeArquivoDistribuicao);
                        break;

                    default:
                        //NF Rejeitada
                        break;
                }
            }
        }
        //Fim Serviços NFCe

        //Serviços MDFe

        private void bt_consulta_status_mdfe_Click(object sender, EventArgs e)
        {
            var xml = new Unimake.Business.DFe.Xml.MDFe.ConsStatServMDFe
            {
                Versao = "3.00",
                TpAmb = TipoAmbiente.Homologacao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CodigoUF = (int)UFBrasil.PR,
                CertificadoDigital = CertificadoSelecionado
            };

            var statusServico = new Unimake.Business.DFe.Servicos.MDFe.StatusServico(xml, configuracao);
            statusServico.Executar();

            MessageBox.Show(statusServico.Result.CStat + " - " + statusServico.Result.XMotivo);
        }

        private void bt_consulta_situacao_mdfe_Click(object sender, EventArgs e)
        {
            var xml = new Unimake.Business.DFe.Xml.MDFe.ConsSitMDFe
            {
                Versao = "3.00",
                TpAmb = TipoAmbiente.Homologacao,
                ChMDFe = "41110479189676000125550010000025721613066220"
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var consultaProtocolo = new Unimake.Business.DFe.Servicos.MDFe.ConsultaProtocolo(xml, configuracao);
            consultaProtocolo.Executar();

            MessageBox.Show(consultaProtocolo.Result.CStat + " - " + consultaProtocolo.Result.XMotivo);
        }

        private void bt_env_mdfe_assinc_Click(object sender, EventArgs e)
        {
            var xml = new Unimake.Business.DFe.Xml.MDFe.EnviMDFe
            {
                Versao = "3.00",
                IdLote = "000000000000001",
                MDFe = new Unimake.Business.DFe.Xml.MDFe.MDFe
                {
                    InfMDFe = new Unimake.Business.DFe.Xml.MDFe.InfMDFe
                    {
                        Versao = "3.00",
                        Ide = new Unimake.Business.DFe.Xml.MDFe.Ide
                        {
                            CUF = UFBrasil.PR,
                            TpAmb = TipoAmbiente.Homologacao,
                            TpEmit = TipoEmitenteMDFe.PrestadorServicoTransporte,
                            Mod = ModeloDFe.MDFe,
                            Serie = 1,
                            NMDF = 861,
                            CMDF = "01722067",
                            Modal = ModalidadeTransporteMDFe.Rodoviario,
                            DhEmi = DateTime.Now,
                            TpEmis = TipoEmissao.Normal,
                            ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                            VerProc = "UNICO V8.0",
                            UFIni = UFBrasil.PR,
                            UFFim = UFBrasil.SP,
                            InfMunCarrega = new List<Unimake.Business.DFe.Xml.MDFe.InfMunCarrega>
                            {
                                new Unimake.Business.DFe.Xml.MDFe.InfMunCarrega
                                {
                                    CMunCarrega = 4118402,
                                    XMunCarrega = "PARANAVAI"
                                }
                            },
                            DhIniViagem = DateTime.Now,
                        },

                        Emit = new Unimake.Business.DFe.Xml.MDFe.Emit
                        {
                            CNPJ = "31905001000109",
                            IE = "9079649730",
                            XNome = "EXATUS MOVEIS EIRELI",
                            XFant = "EXATUS MOVEIS",
                            EnderEmit = new Unimake.Business.DFe.Xml.MDFe.EnderEmit
                            {
                                XLgr = "RUA JOAQUIM F. DE SOUZA",
                                Nro = "01112",
                                XBairro = "VILA TEREZINHA",
                                CMun = 4118402,
                                XMun = "PARANAVAI",
                                CEP = "87706675",
                                UF = UFBrasil.PR,
                                Fone = "04434237530",
                            },
                        },

                        InfModal = new Unimake.Business.DFe.Xml.MDFe.InfModal
                        {
                            VersaoModal = "3.00",
                            Rodo = new Unimake.Business.DFe.Xml.MDFe.Rodo
                            {
                                InfANTT = new Unimake.Business.DFe.Xml.MDFe.InfANTT
                                {
                                    RNTRC = "44957333",
                                    InfContratante = new List<Unimake.Business.DFe.Xml.MDFe.InfContratante>
                                    {
                                        new Unimake.Business.DFe.Xml.MDFe.InfContratante
                                        {
                                            CNPJ = "80568835000181"
                                        },
                                        new Unimake.Business.DFe.Xml.MDFe.InfContratante
                                        {
                                            CNPJ = "10197843000183"
                                        }
                                    }
                                },
                                VeicTracao = new Unimake.Business.DFe.Xml.MDFe.VeicTracao
                                {
                                    CInt = "AXF8500",
                                    Placa = "AXF8500",
                                    Tara = 0,
                                    CapKG = 5000,
                                    Prop = new Unimake.Business.DFe.Xml.MDFe.Prop
                                    {
                                        CNPJ = "31905001000109",
                                        RNTRC = "44957333",
                                        XNome = "EXATUS MOVEIS EIRELI",
                                        IE = "9079649730",
                                        UF = UFBrasil.PR,
                                        TpProp = TipoProprietarioMDFe.Outros
                                    },
                                    Condutor = new List<Unimake.Business.DFe.Xml.MDFe.Condutor>
                                    {
                                        new Unimake.Business.DFe.Xml.MDFe.Condutor
                                        {
                                            XNome = "ADEMILSON LOPES DE SOUZA",
                                            CPF = "27056461832"
                                        }
                                    },
                                    TpRod = TipoRodado.Toco,
                                    TpCar = TipoCarroceriaMDFe.FechadaBau,
                                    UF = UFBrasil.PR
                                },
                            }
                        },

                        InfDoc = new Unimake.Business.DFe.Xml.MDFe.InfDocInfMDFe
                        {
                            InfMunDescarga = new List<Unimake.Business.DFe.Xml.MDFe.InfMunDescarga>
                            {
                                new Unimake.Business.DFe.Xml.MDFe.InfMunDescarga
                                {
                                    CMunDescarga = 3505708,
                                    XMunDescarga = "BARUERI",
                                    InfCTe = new List<Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfCTe>
                                    {
                                        new Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfCTe
                                        {
                                            ChCTe = "41200531905001000109570010000009551708222466"
                                        },
                                        new Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfCTe
                                        {
                                            ChCTe = "41200531905001000109570010000009561308222474"
                                        }
                                    },
                                    InfNFe = new List<Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfNFe>
                                    {
                                        new Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfNFe
                                        {
                                            ChNFe = "12345678901234567890123456789012345678901234",
                                            InfUnidTransp = new List<Unimake.Business.DFe.Xml.MDFe.InfUnidTransp>
                                            {
                                                new Unimake.Business.DFe.Xml.MDFe.InfUnidTransp
                                                {
                                                    IdUnidTransp = "122",
                                                    TpUnidTransp = TipoUnidadeTransporte.RodoviarioReboque,
                                                    LacUnidTransp = new List<Unimake.Business.DFe.Xml.MDFe.LacUnidTransp>
                                                    {
                                                        new Unimake.Business.DFe.Xml.MDFe.LacUnidTransp
                                                        {
                                                            NLacre = "12334"
                                                        }
                                                    },
                                                    InfUnidCarga = new List<Unimake.Business.DFe.Xml.MDFe.InfUnidCarga>
                                                    {
                                                        new Unimake.Business.DFe.Xml.MDFe.InfUnidCarga
                                                        {
                                                            TpUnidCarga = TipoUnidadeCarga.Container,
                                                            IdUnidCarga = "123",
                                                            LacUnidCarga = new List<Unimake.Business.DFe.Xml.MDFe.LacUnidCarga>
                                                            {
                                                                new Unimake.Business.DFe.Xml.MDFe.LacUnidCarga
                                                                {
                                                                    NLacre = "3333333"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new Unimake.Business.DFe.Xml.MDFe.InfMunDescarga
                                {
                                    CMunDescarga = 3550308,
                                    XMunDescarga = "SAO PAULO",
                                    InfCTe = new List<Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfCTe>
                                    {
                                        new Unimake.Business.DFe.Xml.MDFe.InfMunDescargaInfCTe
                                        {
                                            ChCTe = "41200531905001000109570010000009581608222490"
                                        }
                                    }
                                }
                            }
                        },

                        ProdPred = new Unimake.Business.DFe.Xml.MDFe.ProdPred
                        {
                            TpCarga = TipoCargaMDFe.CargaGeral,
                            XProd = "TESTE DE PRODUTO PREDOMINANTE",
                            InfLotacao = new Unimake.Business.DFe.Xml.MDFe.InfLotacao
                            {
                                InfLocalCarrega = new Unimake.Business.DFe.Xml.MDFe.InfLocalCarrega
                                {
                                    CEP = "87302080"
                                },
                                InfLocalDescarrega = new Unimake.Business.DFe.Xml.MDFe.InfLocalDescarrega
                                {
                                    CEP = "25650208"
                                }
                            }
                        },

                        Seg = new List<Unimake.Business.DFe.Xml.MDFe.Seg>
                        {
                            new Unimake.Business.DFe.Xml.MDFe.Seg
                            {
                                InfResp = new Unimake.Business.DFe.Xml.MDFe.InfResp
                                {
                                    RespSeg = ResponsavelSeguroMDFe.EmitenteMDFe,
                                    CNPJ = "31905001000109"
                                },
                                InfSeg = new Unimake.Business.DFe.Xml.MDFe.InfSeg
                                {
                                    XSeg = "PORTO SEGURO",
                                    CNPJ = "61198164000160"
                                },
                                NApol = "053179456362",
                                NAver = new List<string>
                                {
                                    {
                                        "0000000000000000000000000000000000000000"
                                    },
                                    {
                                        "0000000000000000000000000000000000000001"
                                    },
                                }
                            }
                        },
                        Tot = new Unimake.Business.DFe.Xml.MDFe.Tot
                        {
                            QCTe = 3,
                            VCarga = 56599.09,
                            CUnid = CodigoUnidadeMedidaMDFe.KG,
                            QCarga = 2879.00
                        },
                        Lacres = new List<Unimake.Business.DFe.Xml.MDFe.Lacre>
                        {
                            new Unimake.Business.DFe.Xml.MDFe.Lacre
                            {
                                NLacre = "1111111"
                            },
                            new Unimake.Business.DFe.Xml.MDFe.Lacre
                            {
                                NLacre = "22222"
                            }
                        },
                        InfAdic = new Unimake.Business.DFe.Xml.MDFe.InfAdic
                        {
                            InfCpl = "DATA/HORA PREVISTA PARA O INICO DA VIAGEM: 10/08/2020 as 08:00"
                        },
                        InfRespTec = new Unimake.Business.DFe.Xml.MDFe.InfRespTec
                        {
                            CNPJ = "06117473000150",
                            XContato = "Wandrey Mundin Ferreira",
                            Email = "wandrey@unimake.com.br",
                            Fone = "04431414900",
                        },
                    }
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new Unimake.Business.DFe.Servicos.MDFe.Autorizacao(xml, configuracao);
            autorizacao.Executar();

            MessageBox.Show(autorizacao.RetornoWSString);

            if (autorizacao.Result != null)
            {
                MessageBox.Show(autorizacao.Result.XMotivo);

                if (autorizacao.Result.CStat == 103) //103 -> Lote Recebido com Sucesso
                {
                    var xmlRec = new Unimake.Business.DFe.Xml.MDFe.ConsReciMDFe
                    {
                        Versao = "3.00",
                        TpAmb = TipoAmbiente.Homologacao,
                        NRec = autorizacao.Result.InfRec.NRec
                    };

                    var configRec = new Configuracao
                    {
                        TipoDFe = TipoDFe.MDFe,
                        CertificadoDigital = CertificadoSelecionado
                    };

                    var retAutorizacao = new Unimake.Business.DFe.Servicos.MDFe.RetAutorizacao(xmlRec, configRec);
                    retAutorizacao.Executar();

                    //Se autorizado, gravar XML de distribuição
                    if (retAutorizacao.Result.ProtMDFe[0].InfProt.CStat == 100) //100 -> MDFe Autorizado
                    {
                        autorizacao.RetConsReciMDFe = retAutorizacao.Result;
                        autorizacao.GravarXmlDistribuicao(@"d:\testenfe\");
                    }
                    else
                    {
                        //Se rejeitado, realizar tratamento.
                    }

                    //Digamos que eu não consegui o retorno do envio do MDFe que tem o recibo
                    //Como eu faço para finalizar o MDFe ???? Sem o recibo para consultar?
                    autorizacao.RetConsReciMDFe = null;

                    var xmlSit = new Unimake.Business.DFe.Xml.MDFe.ConsSitMDFe
                    {
                        Versao = "3.00",
                        TpAmb = TipoAmbiente.Homologacao,
                        ChMDFe = xml.MDFe.InfMDFe.Chave
                    };

                    var configSit = new Configuracao
                    {
                        TipoDFe = TipoDFe.MDFe,
                        CertificadoDigital = CertificadoSelecionado
                    };

                    var consultaProtocolo = new Unimake.Business.DFe.Servicos.MDFe.ConsultaProtocolo(xmlSit, configSit);
                    consultaProtocolo.Executar();

                    //Se autorizado, gravar XML de distribuição
                    if (consultaProtocolo.Result.CStat == 100)
                    {
                        autorizacao.RetConsSitMDFe.Add(consultaProtocolo.Result);
                        autorizacao.GravarXmlDistribuicao(@"d:\testenfe\");
                    }
                }
            }


            //Fim Serviços MDFe
        }
    }
}