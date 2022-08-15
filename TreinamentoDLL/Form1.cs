using System;
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
using Unimake.Exceptions;
using System.Xml.Linq;

using XmlMDFe = Unimake.Business.DFe.Xml.MDFe;
using XmlNFe = Unimake.Business.DFe.Xml.NFe;
using XmlCTe = Unimake.Business.DFe.Xml.CTe;
using XmlCTeOS = Unimake.Business.DFe.Xml.CTeOS;
using XmlGNRe = Unimake.Business.DFe.Xml.GNRE;

using ServicoCTe = Unimake.Business.DFe.Servicos.CTe;
using ServicoCTeOS = Unimake.Business.DFe.Servicos.CTeOS;
using ServicoGNRe = Unimake.Business.DFe.Servicos.GNRE;
using ServicoMDFe = Unimake.Business.DFe.Servicos.MDFe;
using ServicoNFSe = Unimake.Business.DFe.Servicos.NFSe;
using ServicoNFCe = Unimake.Business.DFe.Servicos.NFCe;
using ServicoNFe = Unimake.Business.DFe.Servicos.NFe;

using DFe = Unimake.Business.DFe;
using DANFe = Unimake.Unidanfe;
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
        private static string PathCertificadoDigital { get; set; } = @"C:\Projetos\Unimake_PV.pfx";

        //Senha de uso do Certificado Digital A1
        private static string SenhaCertificadoDigital { get; set; } = @"12345678";

        //Field para o Certificado Selecionado
        private static X509Certificate2 CertificadoSelecionadoField;

        private static X509Certificate2 CertificadoA3Selecionado;

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
            File.WriteAllText(@"C:\ProjetoLivesDLL\testenfe\retorno\arquivoretorno.xml", autorizacao.RetornoWSString);

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
                    autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");

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

            autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");

            #endregion
        }

        private void bt_env_nfe_assinc_lote_Click(object sender, EventArgs e)
        {
            {
                var xml = new XmlNFe.EnviNFe
                {
                    Versao = "4.00",
                    IdLote = "000000000000001",
                    IndSinc = SimNao.Nao,
                    NFe = new List<XmlNFe.NFe>
                {
                    new XmlNFe.NFe
                    {
                        InfNFe = new List<XmlNFe.InfNFe>
                        {
                            new XmlNFe.InfNFe
                            {
                                Versao = "4.00",
                                Ide = new XmlNFe.Ide
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57980,
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
                                Emit = new XmlNFe.Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new XmlNFe.EnderEmit
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
                                Dest = new XmlNFe.Dest
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlNFe.EnderDest
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
                                Det = CriarDet(),
                                Total = new XmlNFe.Total
                                {
                                    ICMSTot = new XmlNFe.ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 169.80,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 169.80,
                                        VTotTrib = 25.26
                                    }
                                },
                                Transp = new XmlNFe.Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<XmlNFe.Vol>
                                    {
                                        new XmlNFe.Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new XmlNFe.Cobr()
                                {
                                    Fat = new XmlNFe.Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 169.80,
                                        VDesc = 0,
                                        VLiq = 169.80
                                    },
                                    Dup = new List<XmlNFe.Dup>
                                    {
                                        new XmlNFe.Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 169.80
                                        }
                                    }
                                },
                                Pag = new XmlNFe.Pag
                                {
                                    DetPag = new List<XmlNFe.DetPag>
                                    {
                                        new XmlNFe.DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 169.80
                                        }
                                    }
                                },
                                InfAdic = new XmlNFe.InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new XmlNFe.InfRespTec
                                {
                                    CNPJ = "06117473000150",
                                    XContato = "Wandrey Mundin Ferreira",
                                    Email = "wandrey@unimake.com.br",
                                    Fone = "04431414900"
                                }
                            }
                        }
                    },
                    new XmlNFe.NFe
                    {
                        InfNFe = new List<XmlNFe.InfNFe>
                        {
                            new XmlNFe.InfNFe
                            {
                                Versao = "4.00",
                                Ide = new XmlNFe.Ide
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57980,
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
                                Emit = new XmlNFe.Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new XmlNFe.EnderEmit
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
                                Dest = new XmlNFe.Dest
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlNFe.EnderDest
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
                                Det = CriarDet(),
                                Total = new XmlNFe.Total
                                {
                                    ICMSTot = new XmlNFe.ICMSTot
                                    {
                                        VBC = 0,
                                        VICMS = 0,
                                        VICMSDeson = 0,
                                        VFCP = 0,
                                        VBCST = 0,
                                        VST = 0,
                                        VFCPST = 0,
                                        VFCPSTRet = 0,
                                        VProd = 169.80,
                                        VFrete = 0,
                                        VSeg = 0,
                                        VDesc = 0,
                                        VII = 0,
                                        VIPI = 0,
                                        VIPIDevol = 0,
                                        VPIS = 0,
                                        VCOFINS = 0,
                                        VOutro = 0,
                                        VNF = 169.80,
                                        VTotTrib = 25.26
                                    }
                                },
                                Transp = new XmlNFe.Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<XmlNFe.Vol>
                                    {
                                        new XmlNFe.Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new XmlNFe.Cobr()
                                {
                                    Fat = new XmlNFe.Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 169.80,
                                        VDesc = 0,
                                        VLiq = 169.80
                                    },
                                    Dup = new List<XmlNFe.Dup>
                                    {
                                        new XmlNFe.Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 169.80
                                        }
                                    }
                                },
                                Pag = new XmlNFe.Pag
                                {
                                    DetPag = new List<XmlNFe.DetPag>
                                    {
                                        new XmlNFe.DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 169.80
                                        }
                                    }
                                },
                                InfAdic = new XmlNFe.InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new XmlNFe.InfRespTec
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

                var autorizacao = new ServicoNFe.Autorizacao(xml, configuracao);       //Consumindo o serviço
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

                        var xmlRec = new XmlNFe.ConsReciNFe
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

                        var retAutorizacao = new ServicoNFe.RetAutorizacao(xmlRec, configRec);
                        retAutorizacao.Executar();

                        autorizacao.RetConsReciNFe = retAutorizacao.Result;
                        autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");

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
            doc.Load(@"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml");

            var xml = new XmlNFe.EnviNFe
            {
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,
                Versao = "4.00"
            };

            xml.NFe = new List<XmlNFe.NFe>
            {
                XMLUtility.Deserializar<XmlNFe.NFe>(doc)
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new ServicoNFe.Autorizacao(xml, configuracao);
            autorizacao.Executar();

            #endregion
        }

        private void bt_env_evento_cce_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<XmlNFe.Evento>
                {
                    new XmlNFe.Evento
                    {
                        Versao = "1.00",
                        InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoCCE
                        {
                            Versao = "1.00",
                            XCorrecao = "CFOP errada, segue CFOP correta. teste."
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41191006117473000150550010000579281779843610",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.CartaCorrecao,
                            NSeqEvento = 3,
                            VerEvento = "1.00",
                            TpAmb = TipoAmbiente.Homologacao
                        }
                    },
                    new XmlNFe.Evento
                    {
                        Versao = "1.00",
                        InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoCCE
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

            var recepcaoEvento = new ServicoNFe.RecepcaoEvento(xml, configuracao);
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

        private void bt_imprimir_danfe_Click(object sender, EventArgs e)
        {
            var config = new DANFe.Configurations.UnidanfeConfiguration
            {
                Arquivo = @"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml",
                Visualizar = true,
                Imprimir = false,
                EnviaEmail = false,
                Configuracao = "PAISAGEM"
            };

            DANFe.UnidanfeServices.Execute(config);               
        }

        private void bt_exe_config_danfe_Click(object sender, EventArgs e)
        {
            DANFe.UnidanfeServices.ShowConfigurationScreen();
        }

        private void bt_enviar_evento_epec_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<XmlNFe.Evento> {
                        new XmlNFe.Evento
                        {
                            Versao = "1.00",
                            InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoEPEC
                            {
                                COrgaoAutor = UFBrasil.PR,
                                TpAutor = TipoAutor.EmpresaEmitente,
                                VerAplic = "1.00",
                                TpNF = TipoOperacao.Saida,
                                DhEmi = DateTime.Now,
                                IE = "9032000301",
                                Versao = "1.00",
                                Dest = new XmlNFe.DetEventoEPECDest
                                {
                                    CNPJ = "06117473000150",
                                    UF = UFBrasil.PR,
                                    VNF = 86.00,
                                    VICMS = 6.02,
                                    VST = 0.00
                                }
                            })
                            {
                                COrgao = UFBrasil.AN,
                                ChNFe = "41190806117473000150550010000579131943463890",
                                CNPJ = "06117473000150",
                                DhEvento = DateTime.Now,
                                TpEvento = TipoEventoNFe.EPEC,
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

            var recepcaoEvento = new ServicoNFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            var xmlResgatado = XMLUtility.Deserializar<XmlNFe.EnviNFe>("String do meu xml que eu guardei anteriormente");

            var autorizacao = new ServicoNFe.Autorizacao(xmlResgatado, configuracao);
            autorizacao.Executar();
        }

        private void bt_recuperar_xml_nfe_distribuicao_Click(object sender, EventArgs e)
        {
            #region Montar o XML da NFe ou qualquer outro documento (CTe, CTeOS, NFCe ou MDFe)

            var xml = new XmlNFe.EnviNFe
            {
                Versao = "4.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Nao,
                NFe = new List<XmlNFe.NFe>
                {
                    new XmlNFe.NFe
                    {
                        InfNFe = new List<XmlNFe.InfNFe>
                        {
                            new XmlNFe.InfNFe
                            {
                                Versao = "4.00",
                                Ide = new XmlNFe.Ide
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 59903,
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
                                Emit = new XmlNFe.Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new XmlNFe.EnderEmit
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
                                Dest = new XmlNFe.Dest
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlNFe.EnderDest
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
                                Det = CriarDet(),
                                Total = new XmlNFe.Total
                                {
                                    ICMSTot = new XmlNFe.ICMSTot
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
                                Transp = new XmlNFe.Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<XmlNFe.Vol>
                                    {
                                        new XmlNFe.Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new XmlNFe.Cobr()
                                {
                                    Fat = new XmlNFe.Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<XmlNFe.Dup>
                                    {
                                        new XmlNFe.Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new XmlNFe.Pag
                                {
                                    DetPag = new List<XmlNFe.DetPag>
                                    {
                                        new XmlNFe.DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 84.90
                                        }
                                    }
                                },
                                InfAdic = new XmlNFe.InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new XmlNFe.InfRespTec
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

            #endregion

            #region Montar a configuração básica para o envio da NFe

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            #endregion

            #region

            //Montar o objeto do serviço
            var autorizacao = new ServicoNFe.Autorizacao(xml, configuracao);

            //Guardar a chave da nota gerada no banco de dados. Aqui só coloquei em uma variável para que fique registrado como pegar a chave
            var chaveNFe = xml.NFe[0].InfNFe[0].Chave;

            //Salvar o XML da nota em uma pasta ou no banco de dados antes de tentar enviar para a SEFAZ
            File.WriteAllText(@"C:\ProjetoLivesDLL\recursos\NFe.xml", autorizacao.ConteudoXMLAssinado.GetElementsByTagName("NFe")[0].OuterXml);

            //Se conseguiu salvar a nota e está tudo certo, daí sim enviar, conforma abaixo. Mas só envie se tiver garantia que o XML foi salvo acima. Eu vou fazer um teste simples que é ver se o XML existe.
            if (File.Exists(@"C:\ProjetoLivesDLL\recursos\NFe.xml"))
            {
                autorizacao.Executar();
            }

            //Agora não vou pegar o retorno para fingir que a internet caiu bem na hora, então veja que a nota foi enviada e eu não peguei o recibo ou o retorno da autorização, no caso de envio síncrono, como vou finalizar o processo?
            //Vamos para o botão da PARTE 2

            #endregion
        }

        private void bt_recuperar_xml_nfe_distribuicao_2_Click(object sender, EventArgs e)
        {
            #region Resgatar o XML guardado

            var doc = new XmlDocument();
            doc.Load(@"C:\ProjetoLivesDLL\recursos\NFe.xml");

            var xmlNFe = new XmlNFe.EnviNFe
            {
                IdLote = "000000000000002",
                IndSinc = SimNao.Nao,
                Versao = "4.00",
                NFe = new List<XmlNFe.NFe>
                {
                    XMLUtility.Deserializar<XmlNFe.NFe>(doc)
                }
            };

            #endregion

            #region Criar XML da consulta situação da NFe

            var xmlConsSit = new XmlNFe.ConsSitNFe
            {
                ChNFe = xmlNFe.NFe[0].InfNFe[0].Chave,
                TpAmb = TipoAmbiente.Homologacao,
                Versao = "4.00"
            };

            #endregion

            #region Criar uma configuração básica para efetuarmos a consulta situação da NFe pela sua chave.

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            #endregion

            #region Criar o serviço para consumir o serviço de consulta protocolo

            var consultaProtocolo = new ServicoNFe.ConsultaProtocolo(xmlConsSit, configuracao);
            consultaProtocolo.Executar();

            #endregion

            #region Criar objeto para consumir o serviço de envio da NFe para finalizar a nota gerando o arquivo de distribuição

            if (consultaProtocolo.Result.CStat == 100) //NFe Autorizada
            {
                var autorizacao = new ServicoNFe.Autorizacao(xmlNFe, configuracao)
                {
                    RetConsReciNFe = null
                };
                autorizacao.RetConsSitNFes.Add(consultaProtocolo.Result);

                autorizacao.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");
            }
            #endregion
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
                        autorizacao.GravarXmlDistribuicao(@"C:\testenfe\");
                        var docProcNFe = autorizacao.NfeProcResult.GerarXML(); //Gerar o Objeto para pegar a string e gravar em banco de dados
                        MessageBox.Show(autorizacao.NfeProcResult.NomeArquivoDistribuicao);
                        break;

                    default:
                        //NF Rejeitada
                        break;
                }
            }
        }

        private void bt_gerar_nfce_contingencia_Click(object sender, EventArgs e)
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
                                    NNF = 57982,
                                    DhEmi = DateTime.Now,
                                    DhSaiEnt = DateTime.Now,
                                    TpNF = TipoOperacao.Saida,
                                    IdDest = DestinoOperacao.OperacaoInterna,
                                    CMunFG = 4118402,
                                    TpImp = FormatoImpressaoDANFE.NFCe,
                                    TpEmis = TipoEmissao.ContingenciaOffLine,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    FinNFe = FinalidadeNFe.Normal,
                                    IndFinal = SimNao.Sim,
                                    IndPres = IndicadorPresenca.OperacaoPresencial,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "TESTE 1.00",
                                    DhCont = DateTime.Now,
                                    XJust = "Emitido em contingência devido a problemas técnicos."
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
                                Det = new List<Det> {
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
                                            ICMS = new List<ICMS> {
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
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado,
                CSC = "HCJBIRTWGCQ3HVQN7DCA0ZY0P2NYT6FVLPJG",
                CSCIDToken = 2
            };

            #region instanciar a classe do serviço de autorização da NFCe para pegar o conteúdo do XML assinado e 100% preparado para transmissão

            var autorizacao = new ServicoNFCe.Autorizacao(xml, configuracao);

            #endregion instanciar a classe do serviço de autorização da NFCe para pegar o conteúdo do XML assinado e 100% preparado para transmissão

            #region Salvar o XML gerado (Pode-se guardar o conteúdo em string do XML em banco de dados também para resgatar na hora de transmitir.

            StreamWriter streamWriter = null;

            var arqXMLNFCe = Path.Combine(@"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml");

            try
            {
                streamWriter = File.CreateText(arqXMLNFCe);
                streamWriter.Write(autorizacao.ConteudoXMLAssinado.GetElementsByTagName("NFe")[0].OuterXml);
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
            #endregion

            #region Salvar o XML gerado (Pode-se guardar o conteúdo em string do XML em banco de dados também para resgatar na hora de transmitir.

            var config = new DANFe.Configurations.UnidanfeConfiguration
            {
                Arquivo = arqXMLNFCe,
                Copias = 1,
                Visualizar = true,
                Imprimir = false
            };

            DANFe.UnidanfeServices.Execute(config);

            #endregion 
        }

        private void bt_env_nfce_contingencia_offline_Click(object sender, EventArgs e)
        {
            var arquivos = Directory.GetFiles(@"D:\testenfe\NFCeOffline");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado,
                CSC = "HCJBIRTWGCQ3HVQN7DCA0ZY0P2NYT6FVLPJG",
                CSCIDToken = 2
            };

            foreach (var item in arquivos)
            {
                var doc = new XmlDocument();
                doc.Load(item);

                var xml = new XmlNFe.EnviNFe
                {
                    IdLote = "000000000000001",
                    Versao = "4.00",
                    IndSinc = SimNao.Sim,
                    NFe = new List<XmlNFe.NFe>
                    {
                        XMLUtility.Deserializar<XmlNFe.NFe>(doc)
                    }                    
                };

                ServicoNFCe.Autorizacao autorizacao = null;
                try
                {
                    autorizacao = new ServicoNFCe.Autorizacao(xml, configuracao);
                    autorizacao.Executar();
                }
                catch (ValidarXMLException)
                {
                    //Retorno o erro para o usuário
                }
                catch (Exception)
                {
                    //Entro em contingência
                }

                //Fazer os tratamentos dos status, se autorizado ou rejeitado
                MessageBox.Show(autorizacao.Result.ProtNFe.InfProt.CStat.ToString() + " " + autorizacao.Result.ProtNFe.InfProt.XMotivo);

                if (autorizacao != null)
                {
                    if (autorizacao.Result.CStat == 108) //Serviço Paralisado Temporariamente
                    {
                        //Gerar novamente o XML já com as tags de contingência
                    }
                    if (autorizacao.Result.CStat == 109) //Serviço Paralisado sem previsão de retorno
                    {
                        //Gerar novamente o XML já com as tags de contingência
                    }
                }
            }
        }

        private void bt_env_evento_canc_substituicao_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<XmlNFe.Evento>
                {
                    new XmlNFe.Evento
                    {
                        Versao = "1.00",
                        InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoCancSubst
                        {
                            Versao = "1.00",
                            COrgaoAutor = UFBrasil.PR,
                            TpAutor = TipoAutor.EmpresaEmitente,
                            VerAplic = "Unico ERP 9",
                            NProt = "141190000660363",
                            XJust = "Justificativa de teste de cancelamento",
                            ChNFeRef = "00000000000000000000000000000000000000000000"
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41190806117473000150550010000579131943463890",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.CancelamentoPorSubstituicao,
                            NSeqEvento = 1,
                            VerEvento = "1.00",
                            TpAmb = TipoAmbiente.Homologacao
                        }
                    }
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoNFCe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            if (recepcaoEvento.Result.CStat == 128) //Lote de evento processado com sucesso
            {
                switch (recepcaoEvento.Result.RetEvento[0].InfEvento.CStat)
                {
                    case 135: //Evento homologado
                    case 155: //Evento homologado fora do prazo
                        recepcaoEvento.GravarXmlDistribuicao(@"C:\testenfe");
                        break;

                    default:
                        break;
                        //Tratamentos caso haja rejeição
                }
            }
        }

        private void bt_enviar_evento_canc_nfce_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<XmlNFe.Evento>
                {
                    new XmlNFe.Evento
                    {
                        Versao = "1.00",
                        InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoCanc
                        {
                            NProt = "141190000660363",
                            Versao = "1.00",
                            XJust = "Justificativa de teste de cancelamento"
                        })
                        {
                            COrgao = UFBrasil.PR,
                            ChNFe = "41190806117473000150650010000579131943463890",
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
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoNFCe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            if (recepcaoEvento.Result.CStat == 128) //Lote de evento processado com sucesso
            {
                switch (recepcaoEvento.Result.RetEvento[0].InfEvento.CStat)
                {
                    case 135: //Evento homologado
                    case 155: //Evento homologado fora do prazo permitido
                        recepcaoEvento.GravarXmlDistribuicao(@"d:\testenfe");
                        break;

                    default:
                        //Tratamentos necessários quando o evento é rejeitado
                        break;
                }
            }
        }

        private void bt_inutilizacao_nfce_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.InutNFe
            {
                Versao = "4.00",
                InfInut = new XmlNFe.InutNFeInfInut
                {
                    Ano = "19",
                    CNPJ = "06117473000150",
                    CUF = UFBrasil.PR,
                    Mod = ModeloDFe.NFCe,
                    NNFIni = 57919,
                    NNFFin = 57919,
                    Serie = 1,
                    TpAmb = TipoAmbiente.Homologacao,
                    XJust = "Justificativa da inutilizacao de teste"
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFCe,
                CertificadoDigital = CertificadoSelecionado
            };

            var inutilizacao = new ServicoNFCe.Inutilizacao(xml, configuracao);
            inutilizacao.Executar();

            switch (inutilizacao.Result.InfInut.CStat)
            {
                case 102: //Inutilização Homologada
                    inutilizacao.GravarXmlDistribuicao(@"C:\testenfe\");
                    break;

                default:
                    //Tratamentos
                    break;
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
        }

        private void bt_env_mdfe_sinc_Click(object sender, EventArgs e)
        {
            var xml = new XmlMDFe.MDFe
            {
                InfMDFe = new XmlMDFe.InfMDFe
                {
                    Versao = "3.00",
                    Ide = new XmlMDFe.Ide
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
                        InfMunCarrega = new List<XmlMDFe.InfMunCarrega>
                        {
                            new XmlMDFe.InfMunCarrega
                            {
                                CMunCarrega = 4118402,
                                XMunCarrega = "PARANAVAI"
                            }
                        },
                        DhIniViagem = DateTime.Now,
                    },
                    Emit = new XmlMDFe.Emit
                    {
                        CNPJ = "06117473000150",
                        IE = "9032000301",
                        XNome = "TESTE DE ENVIO DE MDFE",
                        XFant = "TESTE DE ENVIO DE MDFE",
                        EnderEmit = new XmlMDFe.EnderEmit
                        {
                            XLgr = "RUA TESTE DE SOUZA",
                            Nro = "01112",
                            XBairro = "JD. SIMARA",
                            CMun = 4118402,
                            XMun = "PARANAVAI",
                            CEP = "87706111",
                            UF = UFBrasil.PR,
                            Fone = "04431421010",
                        },
                    },
                    InfModal = new XmlMDFe.InfModal
                    {
                        VersaoModal = "3.00",
                        Rodo = new XmlMDFe.Rodo
                        {
                            InfANTT = new XmlMDFe.InfANTT
                            {
                                RNTRC = "99778899",
                                InfContratante = new List<XmlMDFe.InfContratante>
                                {
                                    new XmlMDFe.InfContratante
                                    {
                                        CNPJ = "06117473000150"
                                    },
                                    new XmlMDFe.InfContratante
                                    {
                                        CNPJ = "06117473000150"
                                    }
                                }
                            },
                            VeicTracao = new XmlMDFe.VeicTracao
                            {
                                CInt = "ARR5555",
                                Placa = "ARR5555",
                                Tara = 0,
                                CapKG = 5000,
                                Prop = new XmlMDFe.Prop
                                {
                                    CNPJ = "06117473000150",
                                    RNTRC = "12345678",
                                    XNome = "TESTE TESTE TESTE X",
                                    IE = "1234567890",
                                    UF = UFBrasil.PR,
                                    TpProp = TipoProprietarioMDFe.Outros
                                },
                                Condutor = new List<XmlMDFe.Condutor>
                                {
                                    new XmlMDFe.Condutor
                                    {
                                        XNome = "TESTE NOME DO CONDUTOR",
                                        CPF = "00000000000"
                                    }
                                },
                                TpRod = TipoRodado.Toco,
                                TpCar = TipoCarroceriaMDFe.FechadaBau,
                                UF = UFBrasil.PR
                            },
                        }
                    },
                    InfDoc = new XmlMDFe.InfDocInfMDFe
                    {
                        InfMunDescarga = new List<XmlMDFe.InfMunDescarga>
                        {
                            new XmlMDFe.InfMunDescarga
                            {
                                CMunDescarga = 3505708,
                                XMunDescarga = "BARUERI",
                                InfCTe = new List<XmlMDFe.InfMunDescargaInfCTe>
                                {
                                    new XmlMDFe.InfMunDescargaInfCTe
                                    {
                                        ChCTe = "41200506117473000150570010000001113565666658"
                                    }
                                },
                                InfNFe = new List<XmlMDFe.InfMunDescargaInfNFe>
                                {
                                    new XmlMDFe.InfMunDescargaInfNFe
                                    {
                                        ChNFe = "12345678901234567890123456789012345678901234",
                                        InfUnidTransp = new List<XmlMDFe.InfUnidTransp>
                                        {
                                            new XmlMDFe.InfUnidTransp
                                            {
                                                IdUnidTransp = "122",
                                                TpUnidTransp = TipoUnidadeTransporte.RodoviarioReboque,
                                                LacUnidTransp = new List<XmlMDFe.LacUnidTransp>
                                                {
                                                    new XmlMDFe.LacUnidTransp
                                                    {
                                                        NLacre = "12334"
                                                    }
                                                },
                                                InfUnidCarga = new List<XmlMDFe.InfUnidCarga>
                                                {
                                                    new XmlMDFe.InfUnidCarga
                                                    {
                                                        TpUnidCarga = TipoUnidadeCarga.Container,
                                                        IdUnidCarga = "123",
                                                        LacUnidCarga = new List<XmlMDFe.LacUnidCarga>
                                                        {
                                                            new XmlMDFe.LacUnidCarga
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
                            new XmlMDFe.InfMunDescarga
                            {
                                CMunDescarga = 3550308,
                                XMunDescarga = "SAO PAULO",
                                InfCTe = new List<XmlMDFe.InfMunDescargaInfCTe>
                                {
                                    new XmlMDFe.InfMunDescargaInfCTe
                                    {
                                        ChCTe = "41200506117473000150570010000003335655655666"
                                    }
                                }
                            }
                        }
                    },
                    ProdPred = new XmlMDFe.ProdPred
                    {
                        TpCarga = TipoCargaMDFe.CargaGeral,
                        XProd = "TESTE DE PRODUTO PREDOMINANTE",
                        InfLotacao = new XmlMDFe.InfLotacao
                        {
                            InfLocalCarrega = new XmlMDFe.InfLocalCarrega
                            {
                                CEP = "87302080"
                            },
                            InfLocalDescarrega = new XmlMDFe.InfLocalDescarrega
                            {
                                CEP = "25650208"
                            }
                        }
                    },
                    Seg = new List<XmlMDFe.Seg>
                    {
                        new XmlMDFe.Seg
                        {
                            InfResp = new XmlMDFe.InfResp
                            {
                                RespSeg = ResponsavelSeguroMDFe.EmitenteMDFe,
                                CNPJ = "06117473000150"
                            },
                            InfSeg = new XmlMDFe.InfSeg
                            {
                                XSeg = "PORTO SEGURO",
                                CNPJ = "06117473000150"
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
                    Tot = new XmlMDFe.Tot
                    {
                        QCTe = 3,
                        VCarga = 56599.09,
                        CUnid = CodigoUnidadeMedidaMDFe.KG,
                        QCarga = 2879.00
                    },
                    Lacres = new List<XmlMDFe.Lacre>
                    {
                        new XmlMDFe.Lacre
                        {
                            NLacre = "1111111"
                        },
                        new XmlMDFe.Lacre
                        {
                            NLacre = "22222"
                        }
                    },
                    InfAdic = new XmlMDFe.InfAdic
                    {
                        InfCpl = "DATA/HORA PREVISTA PARA O INICO DA VIAGEM: 10/08/2020 as 08:00"
                    },
                    InfRespTec = new XmlMDFe.InfRespTec
                    {
                        CNPJ = "06117473000150",
                        XContato = "TESTE TESTE TESTE",
                        Email = "wandrey@unimake.com.br",
                        Fone = "04431421010",
                    },
                },
            };

            var config = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacaoSinc = new ServicoMDFe.AutorizacaoSinc(xml, config);
            autorizacaoSinc.Executar();

            if (autorizacaoSinc.Result.CStat == 100)
            {
                if (autorizacaoSinc.Result.ProtMDFe.InfProt.CStat == 100)
                {
                    MessageBox.Show(autorizacaoSinc.Result.ProtMDFe.InfProt.NProt);
                }
            }
        }

        private void bt_enviar_evento_cancelamento_Click(object sender, EventArgs e)
        {
            var xml = new XmlMDFe.EventoMDFe
            {
                Versao = "3.00",
                InfEvento = new XmlMDFe.InfEvento(new XmlMDFe.DetEventoCanc
                {
                    NProt = "141200000007987",
                    VersaoEvento = "3.00",
                    XJust = "Teste de cancelamento do MDFe"
                })
                {
                    COrgao = UFBrasil.PR,
                    ChMDFe = "41200210859283000185570010000005671227070615",
                    CNPJ = "10859283000185",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoMDFe.Cancelamento,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoMDFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            MessageBox.Show(recepcaoEvento.RetornoWSString);
            MessageBox.Show(recepcaoEvento.Result.InfEvento.CStat + " - " + recepcaoEvento.Result.InfEvento.XMotivo);

            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo MDFecom situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo MDFe
                case 136: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento ao respectivo MDFe prejudicado
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\testenfe");
                    break;

                default:
                    //Evento rejeitado, fazer devidos tratamentos
                    break;
            }
        }

        private void bt_enviar_evento_encerramento_Click(object sender, EventArgs e)
        {
            var xml = new XmlMDFe.EventoMDFe
            {
                Versao = "3.00",
                InfEvento = new XmlMDFe.InfEvento(new XmlMDFe.DetEventoEncMDFe
                {
                    NProt = "141200000007987",
                    VersaoEvento = "3.00",
                    CMun = 3106200,
                    CUF = UFBrasil.MG,
                    DtEnc = DateTime.Now
                })
                {
                    COrgao = UFBrasil.PR,
                    ChMDFe = "41200210859283000185570010000005671227070615",
                    CNPJ = "10859283000185",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoMDFe.Encerramento,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoMDFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            MessageBox.Show(recepcaoEvento.RetornoWSString);
            MessageBox.Show(recepcaoEvento.Result.InfEvento.CStat + " - " + recepcaoEvento.Result.InfEvento.XMotivo);

            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo MDFecom situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo MDFe
                case 136: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento ao respectivo MDFe prejudicado
                    recepcaoEvento.GravarXmlDistribuicao(@"d:\testenfe");
                    break;

                default:
                    //Evento rejeitado, fazer devidos tratamentos
                    break;
            }
        }

        private void bt_consultar_mdfe_nao_encerrados_Click(object sender, EventArgs e)
        {
            var xmlCons = new XmlMDFe.ConsMDFeNaoEnc
            {
                Versao = "3.00",
                CNPJ = "06117473000150",
                TpAmb = TipoAmbiente.Homologacao,
                XServ = "CONSULTAR NÃO ENCERRADOS"
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado,
                CodigoUF = (int)UFBrasil.PR
            };

            var consultaMDFeNaoEnc = new ServicoMDFe.ConsNaoEnc(xmlCons, configuracao);
            consultaMDFeNaoEnc.Executar();

            MessageBox.Show(consultaMDFeNaoEnc.RetornoWSString);
        }

        private void bt_enviar_evento_pagamento_mdfe_Click(object sender, EventArgs e)
        {
            var xml = new XmlMDFe.EventoMDFe
            {
                Versao = "3.00",
                InfEvento = new XmlMDFe.InfEvento(new XmlMDFe.DetEventoPagtoOperMDFe
                {
                    VersaoEvento = "3.00",
                    EventoPagtoOperMDFe = new XmlMDFe.EventoPagtoOperMDFe
                    {
                        DescEvento = "Pagamento Operacao MDF-e",
                        NProt = "941190000014312",
                        InfViagens = new XmlMDFe.InfViagens
                        {
                            NroViagem = "00001",
                            QtdViagens = "00001"
                        },
                        InfPag = new List<XmlMDFe.PagtoOperMDFeInfPag>
                            {
                                new XmlMDFe.PagtoOperMDFeInfPag
                                {
                                    XNome = "TESTE TRANSPORTE E OPERACOES LTDA",
                                    CNPJ = "00000000000000",
                                    Comp = new List<XmlMDFe.Comp>
                                    {
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.Outros,
                                            VComp = 2000.00,
                                            XComp = "PAGAMENTO DE FRETE"
                                        },
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.ValePedagio,
                                            VComp = 500.00
                                        },
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.Outros,
                                            VComp = 500.00,
                                            XComp = "COMPRA DE PNEUS"
                                        }
                                    },
                                    VContrato = 3000.00,
                                    IndPag = IndicadorPagamento.PagamentoPrazo,
                                    VAdiant = 500.00,
                                    InfPrazo = new List<XmlMDFe.InfPrazo>
                                    {
                                        new XmlMDFe.InfPrazo
                                        {
                                            NParcela = "001",
                                            DVenc = DateTime.Now.AddDays(20),
                                            VParcela = 2000.00
                                        },
                                        new XmlMDFe.InfPrazo
                                        {
                                            NParcela = "002",
                                            DVenc = DateTime.Now.AddDays(40),
                                            VParcela = 500.00
                                        }
                                    },
                                    InfBanc = new XmlMDFe.InfBanc
                                    {
                                        PIX = "+5544993333223"
                                    }
                                }
                            }
                    }
                })
                {
                    COrgao = UFBrasil.PR,
                    ChMDFe = "41200380568835000181580010000007171930099252",
                    CNPJ = "80568835000181",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoMDFe.PagamentoOperacao,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoMDFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            //Gravar o XML de distribuição do evento, se homologado/autorizado
            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        private void bt_enviar_evento_alteracao_mdfe_Click(object sender, EventArgs e)
        {
            var xml = new XmlMDFe.EventoMDFe
            {
                Versao = "3.00",
                InfEvento = new XmlMDFe.InfEvento(new XmlMDFe.DetEventoAlteracaoPagtoServMDFe
                {
                    VersaoEvento = "3.00",

                    EventoAlteracaoPagtoServMDFe = new XmlMDFe.EventoAlteracaoPagtoServMDFe
                    {
                        DescEvento = "Alteracao Pagamento Servico MDFe",
                        NProt = "941190000014312",
                        InfPag = new List<XmlMDFe.AlteracaoPagtoServMDFeInfPag>
                            {
                                new XmlMDFe.AlteracaoPagtoServMDFeInfPag
                                {
                                    XNome = "TESTE TRANSPORTE E OPERACOES LTDA",
                                    CNPJ = "00000000000000",
                                    Comp = new List<XmlMDFe.Comp>
                                    {
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.Outros,
                                            VComp = 2000.00,
                                            XComp = "PAGAMENTO DE FRETE"
                                        },
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.ValePedagio,
                                            VComp = 500.00
                                        },
                                        new XmlMDFe.Comp
                                        {
                                            TpComp = TipoComponenteMDFe.Outros,
                                            VComp = 500.00,
                                            XComp = "COMPRA DE PNEUS"
                                        }
                                    },
                                    VContrato = 3000.00,
                                    IndPag = IndicadorPagamento.PagamentoPrazo,
                                    VAdiant = 500.00,
                                    InfPrazo = new List<XmlMDFe.InfPrazo>
                                    {
                                        new XmlMDFe.InfPrazo
                                        {
                                            NParcela = "001",
                                            DVenc = DateTime.Now.AddDays(20),
                                            VParcela = 2000.00
                                        },
                                        new XmlMDFe.InfPrazo
                                        {
                                            NParcela = "002",
                                            DVenc = DateTime.Now.AddDays(40),
                                            VParcela = 500.00
                                        }
                                    },
                                    InfBanc = new XmlMDFe.InfBanc
                                    {
                                        PIX = "+5544993333223"
                                    }
                                }
                            }
                    }
                })
                {
                    COrgao = UFBrasil.PR,
                    ChMDFe = "41200380568835000181580010000007171930099252",
                    CNPJ = "80568835000181",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoMDFe.AlteracaoPagamentoServico,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoMDFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            //Gravar o XML de distribuição do evento, se homologado/autorizado
            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\ProjetoLivesDLL\testenfe");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        //Fim Serviços MDFe


        //Serviços NFSe
        private void bt_cancelar_nfse_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\CancelarNfseEnvio-ped-cannfse.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeCancelarNfse,
                SchemaVersao = "2.02",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 4204608
            };

            var cancelarNFSe = new ServicoNFSe.CancelarNfse(xmlDoc, configuracao);
            cancelarNFSe.Executar();

            MessageBox.Show(cancelarNFSe.RetornoWSString);

            //var teste = new ServicoNFSe.CancelamentoNfe(xmlDoc, configuracao); -> todos os serviços NFSe
        }

        private void bt_env_rps_sinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\EnviarLoteRpsEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe, 
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeGerarNfse,
                SchemaVersao = "2.02",
                CodigoMunicipio = 4204608
            };

            var gerarNfse = new ServicoNFSe.GerarNfse(xmlDoc, configuracao);
            gerarNfse.Executar();

            MessageBox.Show(gerarNfse.RetornoWSString);
        }

        private void bt_env_lote_rps_sinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\GerarNfseEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeGerarNfse,
                SchemaVersao = "2.02",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 4204608
            };

            var gerarNfse = new ServicoNFSe.GerarNfse(xmlDoc, configuracao);
            gerarNfse.Executar();

            MessageBox.Show(gerarNfse.RetornoWSString);
        }

        private void bt_env_lote_rps_assinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\EnviarLoteRpsEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeRecepcionarLoteRps,
                SchemaVersao = "2.02",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 4204608
            };

            var recepcionarLoteRps = new ServicoNFSe.RecepcionarLoteRps(xmlDoc, configuracao);
            recepcionarLoteRps.Executar();

            MessageBox.Show(recepcionarLoteRps.RetornoWSString);
        }

        private void bt_consultar_lote_rps_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\ConsultarLoteRpsEnvio-ped-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeConsultarLoteRps,
                SchemaVersao = "2.02",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 2933307
            };

            var consultarLoteRps = new ServicoNFSe.ConsultarLoteRps(xmlDoc, configuracao);
            consultarLoteRps.Executar();

            MessageBox.Show(consultarLoteRps.RetornoWSString);
        }

        private void bt_consultar_nfse_por_rps_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\ConsultarNfseRpsEnvio-ped-sitnfse.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe, 
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeConsultarNfsePorRps,
                SchemaVersao = "2.00",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 2933307
            };

            var consultarNfsePorRps = new ServicoNFSe.ConsultarNfsePorRps(xmlDoc, configuracao);
            consultarNfsePorRps.Executar();

            MessageBox.Show(consultarNfsePorRps.RetornoWSString);
        }

        private void bt_substituir_nfse_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\ProjetoLivesDLL\recursos\SubstituirNfseEnvio-ped-substnfse.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeSubstituirNfse,
                SchemaVersao = "2.02",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 4204608
            };

            var substituirNFse = new ServicoNFSe.SubstituirNfse(xmlDoc, configuracao);
            substituirNFse.Executar();

            MessageBox.Show(substituirNFse.RetornoWSString);
        }

        private void bt_xml_nfse_csharp_Click(object sender, EventArgs e)
        {
            var xmlNFSe = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XNamespace xNamespace = "http://www.prefeitura.sp.gov.br/nfe";

            #region Criar a primeira tag <PedidoEnvioRPS> do XML com o namespace

            var tagPedidoEnvioRPS = new XElement(xNamespace + "PedidoEnvioRPS");

            #endregion

            #region Criar o grupo de tag <Cabecalho>

            //Criar tag <Cabecalho>
            var tagCabecalho = new XElement("Cabecalho");

            //Criar o atributo Versao na tag <Cabecalho>
            tagCabecalho.Add(new XAttribute("Versao", "1"));

            //Criar o grupo de tag CPFCNPJRemetente
            var tagCPFCNPJRemetente = new XElement("CPFCNPJRemetente");

            //Criar a tags filhas do grupo <CPFCNPJRemetente>
            tagCPFCNPJRemetente.Add(new XElement("CNPJ", "99999997000100"));

            //Adicionar a tag <CPFCNPJRementente> dentro da tag <Cabecalho>
            tagCabecalho.Add(tagCPFCNPJRemetente);

            //Adicionar a tag <Cabecalho> dentro da tag <PedidoEnvioRPS>
            tagPedidoEnvioRPS.Add(tagCabecalho);

            #endregion

            #region Criar o grupo de tag <RPS>

            //Criar tag <RPS>
            var tagRPS = new XElement("RPS");

            tagRPS.Add(new XElement("Assinatura", "d8Pg/jdA7t5tSaB8Il1d/CMiLGgfFAXzTL9o5stv6TNbhm9I94DIo0/ocqJpGx0KzoEeIQz4RSn99pWX4fiW/aETlNT3u5woqCAyL6U2hSyl/eQfWRYrqFu2zcdc4rsAG/wJbDjNO8y0Pz9b6rlTwkIJ+kMdLo+EWXMnB744olYE721g2O9CmUTvjtBgCfVUgvuN1MGjgzpgyussCOSkLpGbrqtM5+pYMXZsTaEVIIck1baDkoRpLmZ5Y/mcn1/Om1fMyhJVUAkgI5xBrORuotIP7e3+HLJnKgzQQPWCtLyEEyAqUk9Gq64wMayITua5FodaJsX+Eic/ie3kS5m50Q=="));

            //Criar grupo de Tag <ChaveRPS>
            var tagChaveRPS = new XElement("ChaveRPS");

            //Criar tags filhas do grupo <ChaveRPS>
            tagChaveRPS.Add(new XElement("InscricaoPrestador", "39616924"));
            tagChaveRPS.Add(new XElement("SerieRPS", "BB"));
            tagChaveRPS.Add(new XElement("NumeroRPS", "4105"));

            tagRPS.Add(tagChaveRPS);

            //Criar várias tags filhas da tag <RPS>
            tagRPS.Add(new XElement("TipoRPS", "RPS-M"));
            tagRPS.Add(new XElement("DataEmissao", "2015-01-20"));
            tagRPS.Add(new XElement("StatusRPS", "N"));
            tagRPS.Add(new XElement("TributacaoRPS", "T"));
            tagRPS.Add(new XElement("ValorServicos", "20500"));
            tagRPS.Add(new XElement("ValorDeducoes", "5000"));
            tagRPS.Add(new XElement("ValorPIS", "10"));
            tagRPS.Add(new XElement("ValorCOFINS", "10"));
            tagRPS.Add(new XElement("ValorINSS", "10"));
            tagRPS.Add(new XElement("ValorIR", "10"));
            tagRPS.Add(new XElement("ValorCSLL", "10"));
            tagRPS.Add(new XElement("CodigoServico", "7617"));
            tagRPS.Add(new XElement("AliquotaServicos", "0.05"));
            tagRPS.Add(new XElement("ISSRetido", "false"));

            //Criar grupo de tag <CPFCNPJTomador>
            var tagCPFCNPJTomador = new XElement("CPFCNPJTomador");

            //Criar tags filhas do grupo <CPFCNPJTomador>
            tagCPFCNPJTomador.Add(new XElement("CPF", "12345678909"));

            //Adicionar a tag <CPFCNPJTomador> dentro da tag <RPS>
            tagRPS.Add(tagCPFCNPJTomador);

            //Criar mais tags filhas da tag <RPS>
            tagRPS.Add(new XElement("RazaoSocialTomador", "TOMADOR PF"));

            //Criar tag <EnderecoTomador>
            var tagEnderecoTomador = new XElement("EnderecoTomador");

            //Criar tags filhas da tag <EnderecoTomador>
            tagEnderecoTomador.Add(new XElement("TipoLogradouro", "Av"));
            tagEnderecoTomador.Add(new XElement("Logradouro", "Paulista"));
            tagEnderecoTomador.Add(new XElement("NumeroEndereco", "100"));
            tagEnderecoTomador.Add(new XElement("ComplementoEndereco", "Cj 35"));
            tagEnderecoTomador.Add(new XElement("Bairro", "Bela Vista"));
            tagEnderecoTomador.Add(new XElement("Cidade", "3550308"));
            tagEnderecoTomador.Add(new XElement("UF", "SP"));
            tagEnderecoTomador.Add(new XElement("CEP", "1310100"));

            //Adicionar a tag <EnderecoTomador> dentro da tag <RPS>
            tagRPS.Add(tagEnderecoTomador);

            //Criar mais tags filhas da tag <RPS>
            tagRPS.Add(new XElement("EmailTomador", "tomador@teste.com.br"));
            tagRPS.Add(new XElement("Discriminacao", "Desenvolvimento de Web Site Pessoal."));

            #endregion 

            //Adicionar a tag <RPS> dentro da tag <PedidoEnvioRPS>
            tagPedidoEnvioRPS.Add(tagRPS);

            //Adicionar a tag <PedidoEnvioRPS> no xmlNfse
            xmlNFSe.Add(tagPedidoEnvioRPS);

            //Recuperar o conteúdo gerado acima para passar para a DLL no formato XmlDocument, que é o tipo esperado pelo método Executar da DLL
            var conteudoXML = new XmlDocument();
            conteudoXML.CreateXmlDeclaration("1.0", "utf-8", null);

            using (var xmlReader = xmlNFSe.CreateReader())
            {
                conteudoXML.Load(xmlReader);
            }

            //Pronto... só passar a conteudoXML para o método Executar para enviar para a prefeitura de São Paulo-SP.
            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 3550308, //São Paulo-SP
                Servico = Servico.NFSeEnvioRps,
                SchemaVersao = "2.00"
            };

            var envioRps = new ServicoNFSe.EnvioRps(conteudoXML, configuracao);
            envioRps.Executar();
        }

        //Fim Serviços NFSe


        //Serviços CTe e CTeOs
        private void bt_consulta_status_cte_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.ConsStatServCte
            {
                Versao = "3.00",
                TpAmb = TipoAmbiente.Homologacao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CodigoUF = (int)UFBrasil.PR,
                CertificadoDigital = CertificadoSelecionado
            };

            var statusServico = new ServicoCTe.StatusServico(xml, configuracao);
            statusServico.Executar();

            MessageBox.Show(statusServico.Result.CStat + " - " + statusServico.Result.XMotivo);
        }

        private void bt_enviar_cte_assinc_lote_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.EnviCTe
            {
                Versao = "3.00",
                IdLote = "000000000000001",
                CTe = new List<XmlCTe.CTe> {
                        new XmlCTe.CTe
                        {
                            InfCTe = new XmlCTe.InfCTe
                            {
                                Versao = "3.00",
                                Ide = new XmlCTe.Ide
                                {
                                    CUF = UFBrasil.PR,
                                    CCT = "01234567",
                                    CFOP  = "6352",
                                    NatOp = "PREST.SERV.TRANSP.INDUSTR",
                                    Mod = ModeloDFe.CTe,
                                    Serie = 1,
                                    NCT = 868 ,
                                    DhEmi = DateTime.Now,
                                    TpImp = FormatoImpressaoDACTE.NormalPaisagem,
                                    TpEmis = TipoEmissao.Normal,
                                    TpAmb = TipoAmbiente.Homologacao,
                                    TpCTe = TipoCTe.Normal,
                                    ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                                    VerProc = "UNICO V8.0",
                                    CMunEnv = "4118402",
                                    XMunEnv = "PARANAVAI",
                                    UFEnv = UFBrasil.PR,
                                    Modal =  ModalidadeTransporteCTe.Rodoviario,
                                    TpServ = TipoServicoCTe.Normal,
                                    CMunIni = "4118402",
                                    XMunIni = "PARANAVAI",
                                    UFIni = UFBrasil.PR,
                                    CMunFim = "3305109",
                                    XMunFim = "SAO JOAO DE MERITI",
                                    UFFim =  UFBrasil.RJ,
                                    Retira = SimNao.Nao,
                                    IndIEToma = IndicadorIEDestinatario.ContribuinteICMS,
                                    Toma3 =  new XmlCTe.Toma3
                                    {
                                        Toma= TomadorServicoCTe.Remetente,
                                    },
                                },
                                Emit = new XmlCTe.Emit
                                {
                                    CNPJ = "00000000000000",
                                    IE = "9999999999",
                                    XNome = "XXXXXX XXXXXX XXXXXX",
                                    XFant = "XXXXXX XXXXXX",
                                    EnderEmit = new XmlCTe.EnderEmit
                                    {
                                        XLgr = "XXXXXXXXXXXXXXXXXXXXXXX",
                                        Nro = "11111",
                                        XBairro = "XXXXXXXXXXXXXX",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        CEP = "87700000",
                                        UF = UFBrasil.PR,
                                        Fone = "04433333333",
                                    },
                                },
                                Rem = new XmlCTe.Rem
                                {
                                    CNPJ = "00000000000000",
                                    IE = "9999999999",
                                    XNome = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    XFant = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    Fone = "04433333333",
                                    EnderReme = new XmlCTe.EnderReme
                                    {
                                        XLgr = "XXXXXXXXXXXXXXXXXX",
                                        Nro = "9999",
                                        XBairro = "XXXXXXXXXXXXXXX",
                                        CMun = 4118402,
                                        XMun = "PARANAVAI",
                                        CEP = "87700000",
                                        UF = UFBrasil.PR,
                                        CPais = 1058,
                                        XPais = "BRASIL",
                                    }
                                },
                                Dest = new XmlCTe.Dest
                                {
                                    CNPJ = "00000000000000",
                                    IE = "ISENTO",
                                    XNome = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlCTe.EnderDest
                                    {
                                        XLgr = "XXXXXXXXXXXXXXXXXXXXXXXXXXX",
                                        Nro = "55",
                                        XBairro = "CENTRO",
                                        CMun = 3305109,
                                        XMun = "SAO JOAO DE MERITI",
                                        CEP = "25520570",
                                        UF = UFBrasil.RJ,
                                        CPais = 1058,
                                        XPais = "BRASIL",
                                    },
                                },
                                VPrest = new XmlCTe.VPrest
                                {
                                    VTPrest = 50.00,
                                    VRec = 50.00,
                                    Comp = new List<XmlCTe.Comp>
                                    {
                                        new XmlCTe.Comp
                                        {
                                            XNome = "FRETE VALOR",
                                            VComp = 50.00,
                                        },
                                    },
                                },
                                Imp = new XmlCTe.Imp
                                {
                                    ICMS = new XmlCTe.ICMS
                                    {
                                        ICMSSN = new XmlCTe.ICMSSN
                                        {
                                            CST = "90",
                                            IndSN = SimNao.Sim,
                                        }
                                    }
                                },
                                InfCTeNorm = new XmlCTe.InfCTeNorm
                                {
                                    InfCarga = new XmlCTe.InfCarga
                                    {
                                        VCarga = 6252.96,
                                        ProPred = "xxxxxxx",
                                        InfQ = new List<XmlCTe.InfQ>
                                        {
                                            new XmlCTe.InfQ
                                            {
                                                CUnid = CodigoUnidadeMedidaCTe.KG,
                                                TpMed ="PESO BRUTO",
                                                QCarga = 320.0000,
                                            },
                                            new XmlCTe.InfQ
                                            {
                                                CUnid = CodigoUnidadeMedidaCTe.UNIDADE,
                                                TpMed ="UNIDADE",
                                                QCarga = 1.0000,
                                            },
                                        },
                                    },
                                    InfDoc = new XmlCTe.InfDoc
                                    {
                                        InfNFe = new List<XmlCTe.InfNFe>
                                        {
                                            new XmlCTe.InfNFe
                                            {
                                                Chave = "41444444444444444444444444444444444444444441"
                                            },
                                        },
                                    },
                                    InfModal = new XmlCTe.InfModal
                                    {
                                        VersaoModal="3.00",
                                        Rodo = new XmlCTe.Rodo
                                        {
                                            RNTRC = "44444444",
                                            Occ = new List<XmlCTe.Occ>
                                            {
                                                new XmlCTe.Occ
                                                {
                                                    NOcc = 810,
                                                    DEmi = DateTime.Now,
                                                    EmiOcc = new XmlCTe.EmiOcc
                                                    {
                                                        CNPJ = "00000000000000",
                                                        CInt = "0000000000",
                                                        IE = "9999999999",
                                                        UF = UFBrasil.PR,
                                                        Fone = "04433333333",
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                                InfRespTec = new XmlCTe.InfRespTec
                                {
                                    CNPJ = "00000000000000",
                                    XContato = "XXXXXXXXXXXXXXXXXXXXXXX",
                                    Email= "teste@gmail.com",
                                    Fone = "04433333333",
                                },
                            },
                        },
                    }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            var autorizacao = new ServicoCTe.Autorizacao(xml, configuracao);
            autorizacao.Executar();

            if(autorizacao.Result != null)
            {
                if(autorizacao.Result.CStat == 103) //Lote recebido com sucesso
                {
                    var xmlRec = new XmlCTe.ConsReciCTe
                    {
                        Versao = "3.00",
                        TpAmb = TipoAmbiente.Homologacao,
                        NRec = autorizacao.Result.InfRec.NRec
                    };

                    var configRec = new Configuracao
                    {
                        TipoDFe = TipoDFe.CTe,
                        CertificadoDigital = CertificadoSelecionado
                    };

                    var retAutorizacao = new ServicoCTe.RetAutorizacao(xmlRec, configuracao);
                    retAutorizacao.Executar();

                    if (retAutorizacao.Result.ProtCTe[0].InfProt.CStat == 100) //CTe Autorizado
                    {
                        autorizacao.RetConsReciCTe = retAutorizacao.Result;
                        autorizacao.GravarXmlDistribuicao(@"C:\testenfe");
                    }
                } 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bt_enviar_cteos_sinc_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTeOS.CTeOS
            {
                Versao = "3.00",
                InfCTe = new XmlCTeOS.InfCTe
                {
                    Versao = "3.00",
                    Ide = new XmlCTeOS.Ide
                    {
                        CUF = UFBrasil.PR,
                        CCT = "12356488",
                        CFOP = "6352",
                        NatOp = "PREST.SERV.TRANSP.INDUSTR",
                        Mod = ModeloDFe.CTeOS,
                        Serie = 1,
                        NCT = 861,
                        DhEmi = DateTime.Now,
                        TpImp = FormatoImpressaoDACTE.NormalPaisagem,
                        TpEmis = TipoEmissao.Normal,
                        TpAmb = TipoAmbiente.Homologacao,
                        TpCTe = TipoCTe.Normal,
                        ProcEmi = ProcessoEmissao.AplicativoContribuinte,
                        VerProc = "UNICO V8.0",
                        CMunEnv = "4118402",
                        XMunEnv = "PARANAVAI",
                        UFEnv = UFBrasil.PR,
                        Modal = ModalidadeTransporteCTe.Rodoviario,
                        TpServ = TipoServicoCTeOS.TransportePessoas,
                        CMunIni = "4118402",
                        XMunIni = "PARANAVAI",
                        UFIni = UFBrasil.PR,
                        CMunFim = "3305109",
                        XMunFim = "SAO JOAO DE MERITI",
                        UFFim = UFBrasil.RJ,
                        IndIEToma = IndicadorIEDestinatario.ContribuinteICMS,
                        InfPercurso = new List<XmlCTeOS.InfPercurso>
                            {
                                new XmlCTeOS.InfPercurso
                                {
                                    UFPer = UFBrasil.SP
                                }
                            }
                    },
                    Compl = new XmlCTeOS.Compl
                    {
                        XObs = "Teste de observacao",
                        ObsCont = new List<XmlCTeOS.ObsCont>
                            {
                                new XmlCTeOS.ObsCont
                                {
                                    XCampo = "LEI DA TRANSPARENCIA",
                                    XTexto = "O valor aproximado de tributos incidentes sobre o preco deste servico e de R$ 177.33 .(0) Fonte: IBPT"
                                }
                            },
                    },
                    Emit = new XmlCTeOS.Emit
                    {
                        CNPJ = "00000000000000",
                        IE = "0000000000",
                        XNome = "XXXXXXXXXXXXXXXXXXXX",
                        XFant = "XXXXXXXXXXXXX",
                        EnderEmit = new XmlCTeOS.EnderEmit
                        {
                            XLgr = "XXXXXXXXXXXXXXXXXXXXXXX",
                            Nro = "00001",
                            XBairro = "XXXXXXXXXXXXXX",
                            CMun = 4118402,
                            XMun = "PARANAVAI",
                            CEP = "87700000",
                            UF = UFBrasil.PR,
                            Fone = "04444444444",
                        },
                    },
                    Toma = new XmlCTeOS.Toma
                    {
                        CNPJ = "00000000000000",
                        IE = "0000000000",
                        XNome = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                        XFant = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                        Fone = "04434225480",
                        EnderToma = new XmlCTeOS.EnderToma
                        {
                            XLgr = "XXXXXXXXXXXXXXXXXXXXXXX",
                            Nro = "00001",
                            XBairro = "XXXXXXXXXXXXXX",
                            CMun = 4118402,
                            XMun = "PARANAVAI",
                            CEP = "87700000",
                            UF = UFBrasil.PR,
                            CPais = 1058,
                            XPais = "BRASIL",
                        }
                    },
                    VPrest = new XmlCTeOS.VPrest
                    {
                        VTPrest = 2845.15,
                        VRec = 2845.15,
                        Comp = new List<XmlCTeOS.Comp>
                            {
                                new XmlCTeOS.Comp
                                {
                                    XNome = "VIAGEM TURISMO",
                                    VComp = 2356.00,
                                },
                                new XmlCTeOS.Comp
                                {
                                    XNome = "PEDAGIO",
                                    VComp = 311.82,
                                },
                            },
                    },
                    Imp = new XmlCTeOS.Imp
                    {
                        ICMS = new XmlCTeOS.ICMS
                        {
                            ICMS00 = new XmlCTeOS.ICMS00
                            {
                                CST = "00",
                                VBC = 2533.33,
                                PICMS = 7.00,
                                VICMS = 177.33
                            }
                        },
                        VTotTrib = 177.33,
                        InfTribFed = new XmlCTeOS.InfTribFed
                        {
                            VPIS = 30.00,
                            VCOFINS = 3.00,
                            VIR = 3.00,
                            VINSS = 3.00,
                            VCSLL = 3.00
                        }
                    },
                    InfCTeNorm = new XmlCTeOS.InfCTeNorm
                    {
                        InfServico = new XmlCTeOS.InfServico
                        {
                            XDescServ = "TRANSPORTES DE PESSOINHAS",
                            InfQ = new XmlCTeOS.InfQ
                            {
                                QCarga = 1
                            }
                        },
                        Seg = new List<XmlCTeOS.Seg>
                            {
                                new XmlCTeOS.Seg
                                {
                                    RespSeg = ResponsavelSeguroCTeOS.EmitenteCTeOS
                                }
                            },
                        InfModal = new XmlCTeOS.InfModal
                        {
                            VersaoModal = "3.00",
                            RodoOS = new XmlCTeOS.RodoOS
                            {
                                TAF = "999999999999",
                            }
                        }
                    },
                    AutXML = new List<XmlCTeOS.AutXML>
                        {
                            new XmlCTeOS.AutXML
                            {
                             CNPJ = "99999999999999",
                            },
                            new XmlCTeOS.AutXML
                            {
                             CNPJ = "99999999999998",
                            }
                        },
                    InfRespTec = new XmlCTeOS.InfRespTec
                    {
                        CNPJ = "00000000000000",
                        XContato = "XXXXXXXXXXXXXXXXXXXXXXX",
                        Email = "teste@gmail.com",
                        Fone = "04433333333",
                    },
                },
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTeOS,
                CertificadoDigital = CertificadoSelecionado   
            };

            var autorizacao = new ServicoCTeOS.Autorizacao(xml, configuracao);
            autorizacao.Executar();

            if(autorizacao.Result.ProtCTe != null)
            {
                if (autorizacao.Result.CStat == 103) //Lote recebido com sucesso
                {
                    if(autorizacao.Result.ProtCTe.InfProt.CStat == 100)
                    {
                        autorizacao.GravarXmlDistribuicao(@"C:\testenfe\");
                    }
                }
            }
        }

        private void bt_evento_canc_cte_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.EventoCTe
            {
                Versao = "3.00",
                InfEvento = new XmlCTe.InfEvento(new XmlCTe.DetEventoCanc
                {
                    VersaoEvento = "3.00",
                    NProt = "141200000001111",
                    XJust = "Justificativa de teste de cancelamento"
                })
                {
                    COrgao = UFBrasil.PR,
                    ChCTe = "41200211111111111111111111111111111111111115",
                    CNPJ = "11111111111111",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoCTe.Cancelamento,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoCTe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\testecte\");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        private void bt_evento_canc_cteos_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.EventoCTe
            {
                Versao = "3.00",
                InfEvento = new XmlCTe.InfEvento(new XmlCTe.DetEventoCanc
                {
                    VersaoEvento = "3.00",
                    NProt = "141200000001111",
                    XJust = "Justificativa de teste de cancelamento"
                })
                {
                    COrgao = UFBrasil.PR,
                    ChCTe = "41200211111111111111111111111111111111111115",
                    CNPJ = "11111111111111",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoCTe.Cancelamento,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoCTe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\testecte\");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        private void bt_evento_cce_e_cte_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.EventoCTe
            {
                Versao = "3.00",
                InfEvento = new XmlCTe.InfEvento(new XmlCTe.DetEventoCCE
                {
                    VersaoEvento = "3.00",
                    EventoCCeCTe = new XmlCTe.EventoCCeCTe
                    {
                        InfCorrecao = new List<XmlCTe.InfCorrecao>
                        {
                            new XmlCTe.InfCorrecao
                            {
                                GrupoAlterado = "ide",
                                CampoAlterado = "cfop",
                                ValorAlterado = "6353",
                                NroItemAlterado = ""
                            },
                            new XmlCTe.InfCorrecao
                            {
                                GrupoAlterado = "ide",
                                CampoAlterado = "cfop",
                                ValorAlterado = "6353",
                                NroItemAlterado = ""
                            }
                        }
                    }
                })
                {
                    COrgao = UFBrasil.PR,
                    ChCTe = "41200211111111111111111111111111111111111115",
                    CNPJ = "11111111111111",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoCTe.CartaCorrecao,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoCTe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            //Gravar o XML de distribuição se o evento foi homologada
            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\testecte\");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        private void bt_evento_cce_e_cteos_Click(object sender, EventArgs e)
        {
            var xml = new XmlCTe.EventoCTe
            {
                Versao = "3.00",
                InfEvento = new XmlCTe.InfEvento(new XmlCTe.DetEventoCCE
                {
                    VersaoEvento = "3.00",
                    EventoCCeCTe = new XmlCTe.EventoCCeCTe
                    {
                        InfCorrecao = new List<XmlCTe.InfCorrecao>
                        {
                            new XmlCTe.InfCorrecao
                            {
                                GrupoAlterado = "ide",
                                CampoAlterado = "cfop",
                                ValorAlterado = "6353",
                                NroItemAlterado = ""
                            },
                            new XmlCTe.InfCorrecao
                            {
                                GrupoAlterado = "ide",
                                CampoAlterado = "cfop",
                                ValorAlterado = "6353",
                                NroItemAlterado = ""
                            }
                        }
                    }
                })
                {
                    COrgao = UFBrasil.PR,
                    ChCTe = "41200211111111111111111111111111111111111115",
                    CNPJ = "11111111111111",
                    DhEvento = DateTime.Now,
                    TpEvento = TipoEventoCTe.CartaCorrecao,
                    NSeqEvento = 1,
                    TpAmb = TipoAmbiente.Homologacao
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            var recepcaoEvento = new ServicoCTe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            //Gravar o XML de distribuição se o evento foi homologada
            switch (recepcaoEvento.Result.InfEvento.CStat)
            {
                case 134: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respectivo CT-e com situação diferente de Autorizada.
                case 135: //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento no respetivo CTe.
                case 136: //Recebido pelo Sistema de Registro de Eventos – vinculação do evento ao respectivo CT-e prejudicado.
                    recepcaoEvento.GravarXmlDistribuicao(@"C:\testecte\");
                    break;

                default:
                    //Quando o evento é rejeitado pela Sefaz.
                    break;
            }
        }

        //Fim Serviços CTe e CTeOs


        //Serviços GNRE

        private void bt_consulta_confg_uf_Click(object sender, EventArgs e)
        {
            var xml = new XmlGNRe.TConsultaConfigUf
            {
                Ambiente = TipoAmbiente.Homologacao,
                UF = UFBrasil.RS,
                Receita = new XmlGNRe.Receita
                {
                    Courier = SimNaoLetra.Nao,
                    Value = 100064
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.GNRE,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoArquivo = @"C:\Projetos\Unimake_PV.pfx",
                CertificadoSenha = "12345678"
            };

            var consultaConfigUF = new ServicoGNRe.ConsultaConfigUF(xml, configuracao);
            consultaConfigUF.Executar();
        }

        private void bt_enviar_xml_gnre_Click(object sender, EventArgs e)
        {
            var xml = new XmlGNRe.TLoteGNRE
            {
                Guias = new XmlGNRe.Guias
                {
                    TDadosGNRE = new List<XmlGNRe.TDadosGNRE>
                    {
                        new XmlGNRe.TDadosGNRE
                        {
                            Versao = "2.00",
                            UfFavorecida = UFBrasil.PR,
                            TipoGNRE = TipoGuiaGNRE.Simples,
                            ContribuinteEmitente = new XmlGNRe.ContribuinteEmitente
                            {
                                Identificacao = new XmlGNRe.Identificacao
                                {
                                    CNPJ = "07666666000166",
                                    IE = "9335665656"
                                },
                                RazaoSocial = "TESTE EMPRESA PARA ENVIO DA GNRE",
                                Endereco = "XXX XXXXXXX XXXXX",
                                Municipio = "04808",
                                UF = UFBrasil.PR,
                                CEP= "90399899",
                                Telefone = "04456566566"
                            },

                            ItensGNRE = new XmlGNRe.ItensGNRE
                            {
                                Item = new List<XmlGNRe.Item>
                                {
                                    new XmlGNRe.Item
                                    {
                                        Receita = "100099",
                                        DocumentoOrigem = new XmlGNRe.DocumentoOrigem
                                        {
                                            Tipo = "10",
                                            Value = "41210807666666000166550010001234551123455553"
                                        },
                                        DataVencimento = DateTime.Now,
                                        Valor = new List<XmlGNRe.Valor>
                                        {
                                            new XmlGNRe.Valor
                                            {
                                                Tipo = XmlGNRe.Valor.ItemValorTipo.Item11,
                                                ValorOriginal = 116.24
                                            }
                                        },
                                        ContribuinteDestinatario = new XmlGNRe.ContribuinteDestinatario
                                        {
                                            Identificacao = new XmlGNRe.Identificacao
                                            {
                                                IE = "1236566556"
                                            },
                                        },
                                    }
                                }
                            },

                            ValorGNRE = 30.00,
                            DataPagamento = DateTime.Now
                        }
                    }
                }
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.GNRE,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoArquivo = @"C:\Projetos\Unimake_PV.pfx",
                CertificadoSenha = "12345678",
                TipoAmbiente = TipoAmbiente.Homologacao,
                CodigoUF = 41 //Paraná
            };

            var loteRecepcao = new ServicoGNRe.LoteRecepcao(xml, configuracao);
            loteRecepcao.Executar();

            if (loteRecepcao.Result.SituacaoRecepcao.Codigo == "100")
            {
                //Aguardar 30 segundos para consultar o resultado do lote, é o que solicitam no manual.
                //Thread.Sleep(30000);

                var xmlCons = new XmlGNRe.TConsLoteGNRE
                {
                    Ambiente = TipoAmbiente.Homologacao,
                    NumeroRecibo = loteRecepcao.RetornoWSXML.GetElementsByTagName("ns1:numero")[0].InnerText,
                    IncluirPDFGuias = SimNaoLetra.Sim,
                    IncluirArquivoPagamento = SimNaoLetra.Nao
                };

                var configCons = new Configuracao
                {
                    TipoDFe = TipoDFe.GNRE,
                    TipoEmissao = TipoEmissao.Normal,
                    CertificadoArquivo = @"C:\Projetos\Unimake_PV.pfx",
                    CertificadoSenha = "12345678",
                    TipoAmbiente = TipoAmbiente.Homologacao,
                    CodigoUF = 41 //Paraná
                };

                var consultaResultadoLote = new ServicoGNRe.ConsultaResultadoLote(xmlCons, configCons);
                consultaResultadoLote.Executar();

                switch (consultaResultadoLote.Result.SituacaoProcess.Codigo)
                {
                    case "400": //Lote recebido, aguardando processamento
                    case "401": //Lote em processamento
                        //Tentar consultar mais tarde
                        break;

                    case "402": //Lote processado com sucesso
                        try
                        {
                            consultaResultadoLote.GravarXmlRetorno(@"C:\ProjetoLivesDLL\testenfe", xmlCons.NumeroRecibo + "-ret-gnre.xml");
                            consultaResultadoLote.GravarPDFGuia(@"C:\ProjetoLivesDLL\testenfe", "GuiaGNRE.pdf");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;

                    case "403": //Lote processado com pendências ou Lote com pendência de tempo de processamento. As Guias com situação 4 (campo < situacaoGuia > para a versão 2.00) podem levar em média 20 minutos, e no máximo 1 hora para serem processadas.
                        //Analisar pendencias
                        break;

                    case "404": //Erro no processamento do lote.
                        //Enviar lote novamente.
                        break;
                }
            }
        }

        private void bt_consultar_resultado_lote_gnre_Click(object sender, EventArgs e)
        {
            var xml = new XmlGNRe.TConsLoteGNRE
            {
                Ambiente = TipoAmbiente.Homologacao,
                NumeroRecibo = "1234567890",
                IncluirPDFGuias = SimNaoLetra.Sim,
                IncluirArquivoPagamento = SimNaoLetra.Nao,
                IncluirNoticias = SimNaoLetra.Nao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.GNRE,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoArquivo = @"C:\Projetos\Unimake_PV.pfx",
                CertificadoSenha = "12345678",
                TipoAmbiente = TipoAmbiente.Homologacao,
                CodigoUF = 41 //Paraná
            };

            var consultaResultadoLote = new ServicoGNRe.ConsultaResultadoLote(xml, configuracao);
            consultaResultadoLote.Executar();

            switch (consultaResultadoLote.Result.SituacaoProcess.Codigo)
            {
                case "400": //Lote recebido, aguardando processamento
                case "401": //Lote em processamento
                    //Tentar consultar mais tarde
                    break;

                case "402": //Lote processado com sucesso.
                    consultaResultadoLote.GravarXmlRetorno(@"C:\ProjetoLivesDLL\testenfe", xml.NumeroRecibo + "-ret-gnre.xml");
                    consultaResultadoLote.GravarPDFGuia(@"C:\ProjetoLivesDLL\testenfe", "GuiaGNRE.pdf");
                    break;

                default:
                    //Rejeições diversas
                    break;
            }
        }

        //Fim Serviços GNRE


        //Serviços Diversos

        private void bt_carregar_cert_a3_Click(object sender, EventArgs e)
        {
            //var cert = CertificadoDigital();
            //CertificadoA3Selecionado = cert.Selecionar();

            //if (ClsX509Certificate2Extension.IsA3(CertificadoA3Selecionado))
            //{
            //    ClsX509Certificate2Extension.SetPinPrivateKey(CertificadoA3Selecionado, "12345678");
            //}

            //Para funcionar o método IsA3 e SetPinPrivateKey, abaixo, tem que dar o using no name space a seguir "using Unimake.Business.DFe.Security" 
            //caso contrário não vai localizar a extensão criada para a X509Certificate2 na DLL.
            //Testa se é um certificado A3 ou não e se for vai setar o PIN, caso eu não passe o PIN, no momento do uso do certificado vai abrir aquela tela do administrador do token para informar manualmente.
            //if (CertificadoA3Selecionado.IsA3())
            //{
                //Setar a senha do PIN do certificado A3
            //    CertificadoA3Selecionado.SetPinPrivateKey("1234");
            //}
        }

        private void bt_imprimir_danfe_etiqueta_Click(object sender, EventArgs e)
        {
            var config = new DANFe.Configurations.UnidanfeConfiguration
            {
                Arquivo = @"C:\Projetos\UniNFe\exemplos\NFe e NFCe 4.00\NFe\41170706117473000150550010000463191912756548-procNFe.xml",
                Visualizar = true,
                Imprimir = false,
                EnviaEmail = false,
                Configuracao = "DANFE_ETIQ" //Informar a configuração específico para o DANFe etiqueta, aqui está o segredo.
            };

            DANFe.UnidanfeServices.Execute(config);
        }

        private void button1_Click(object sender, EventArgs e)  //Formas de Trabalhar com Certificado Digital
        {
            var certificado = new CertificadoDigital();

            #region Certificado A1 e A3 - Abrir tela para selecionar o certificado digital que eu desejo trabalhar, certificado que está instalado no repositório do windows

            var certSel1 = certificado.AbrirTelaSelecao();

            var config1 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel1
            };

            //Você pode salvar o Thumbprint ou SerialNumber do certificado para salvar em sua base de dados para resgatar ele no futuro no repositório do windows sem precisar abrir tela para selecionar novamente.
            var thumbprint = certSel1.Thumbprint;
            var serialNumber = certSel1.SerialNumber;

            #endregion

            #region Somente certificado A1 - Carregar o certificado digital direto do arquivo .PFX.

            var certSel2 = certificado.CarregarCertificadoDigitalA1(@"C:\Projetos\Unimake_PV.pfx", "12345678");

            var config2 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel2
            };

            //Outra forma de configurar pegando o .PFX sem precisar carregar, ou seja, a propria configuração já carrega o A1.
            //Não recomendamos pq toda vez vai fazer acesso ao HD, melhor é carregar na forma anterior e deixar em uma variável global e só utilizar quando precisar.
            var config22 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoArquivo = @"C:\Projetos\Unimake_PV.pfx",
                CertificadoSenha = "12345678"
            };

            #endregion

            #region Certificado A1 e A3 - Buscar o certificado digital, instalado no repositório do windows, pelo Serial Number

            var certSel3 = certificado.BuscarCertificadoDigital(serialNumber);
            MessageBox.Show(certSel3.Subject);

            var config3 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel3
            };

            #endregion

            #region Certificado A1 e A3 - Buscar o certificado digital, instalado no repositório do windows, pelo ThumbPrint

            var certSel4 = certificado.BuscarCertificadoDigital(thumbprint);

            var config4 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel4
            };

            #endregion

            #region Somente certificado A1 - Criando uma array bytes do arquivo do certificado para gravar em banco de dados (visando maior segurança) para resgatar o conteúdo da base de dados.

            var certificadoByte = certificado.ToByteArray(@"C:\Projetos\Unimake_PV.pfx");

            //Agora você pode gravar o conteúdo da "certificadoByte" no banco de dados

            //Recuperar o certificado para uso a partir de uma array byte
            var certSel5 = certificado.CarregarCertificadoDigitalA1(certificadoByte, "12345678");

            var config5 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel5
            };

            #endregion

            #region Somente certificado A1 - Criando um Base64 do arquivo do certificado para gravar em banco de dados (visando maior segurança) para resgatar o conteúdo da base de dados.

            var certificadoBase64 = certificado.ToBase64(@"C:\Projetos\Unimake_PV.pfx");

            //Agora você pode gravar o conteúdo da "certificadoBase64" no banco dados

            //Recuperar o certificado digital a partir de um Base64
            var certSel6 = certificado.FromBase64(certificadoBase64, "12345678");

            var config6 = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = certSel6
            };

            #endregion
        }

        private void bt_distribuicao_dfe_Click(object sender, EventArgs e)
        {
            var nsu = "000000000000000"; //Começar com o NSU 0 quando não tem o ultNSU

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            pb_consulta_dfe.Visible = true;
            pb_consulta_dfe.Minimum = 0;
            Application.DoEvents();
            pb_consulta_dfe.Refresh();

            while (true)
            {
                var xml = new XmlNFe.DistDFeInt
                {
                    Versao = "1.01",
                    TpAmb = TipoAmbiente.Homologacao,
                    CNPJ = "06117473000150",
                    DistNSU = new XmlNFe.DistNSU
                    {
                        UltNSU = nsu
                    }
                };

                var distribuicaoDFe = new ServicoNFe.DistribuicaoDFe(xml, configuracao);
                distribuicaoDFe.Executar();

                #region Atualizar ProgressBar

                if (pb_consulta_dfe.Maximum != Convert.ToInt32(distribuicaoDFe.Result.MaxNSU))
                {
                    pb_consulta_dfe.Maximum = Convert.ToInt32(distribuicaoDFe.Result.MaxNSU);
                }

                pb_consulta_dfe.Value = Convert.ToInt32(distribuicaoDFe.Result.UltNSU);
                pb_consulta_dfe.Refresh();
                Application.DoEvents();

                #endregion Atualizar ProgressBar

                if (distribuicaoDFe.Result.CStat == 138) // Documentos localizados e 137 = Não tem documentos
                {
                    var folder = @"C:\testenfe\doczip";

                    //Salvar XMLs do docZIP no HD
                    distribuicaoDFe.GravarXMLDocZIP(folder, true);
                }

                nsu = distribuicaoDFe.Result.UltNSU; //Salvar o ultNSU para usar na próxima consulta
                //Importante salvar o conteúdo de "nsu" na base de dados.

                if (Convert.ToInt64(distribuicaoDFe.Result.UltNSU) >= Convert.ToInt64(distribuicaoDFe.Result.MaxNSU))
                {
                    break;
                }

                pb_consulta_dfe.Visible = false;
                Application.DoEvents();
            }
        }

        private void bt_manisfestacao_dest_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnvEvento
            {
                Versao = "1.00",
                IdLote = "000000000000001",
                Evento = new List<XmlNFe.Evento> {
                    new XmlNFe.Evento
                    {
                        Versao = "1.00",
                        InfEvento = new XmlNFe.InfEvento(new XmlNFe.DetEventoManif
                        {
                            Versao = "1.00",
                            DescEvento = "Operacao nao Realizada", //Pode ser: "Ciencia da Operacao" / "Confirmacao da Operacao" / "Desconhecimento da Operacao" / "Operacao nao Realizada"
                            XJust = "Justificativa para manifestação da NFe de teste"
                        })
                        {
                            COrgao = UFBrasil.AN,
                            ChNFe = "41200211111111111111111111111111111111111115",
                            CNPJ = "06117473000150",
                            DhEvento = DateTime.Now,
                            TpEvento = TipoEventoNFe.ManifestacaoOperacaoNaoRealizada,
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

            var recepcaoEvento = new ServicoNFe.RecepcaoEvento(xml, configuracao);
            recepcaoEvento.Executar();

            //Gravar o XML de distribuição do evento
            if (recepcaoEvento.Result.CStat == 128) //128 = Lote de evento processado com sucesso
            {
                switch (recepcaoEvento.Result.RetEvento[0].InfEvento.CStat)
                {
                    case 135: //Evento homologado com vinculação da respectiva NFe
                        recepcaoEvento.GravarXmlDistribuicao(@"C:\testenfe\");
                        break;

                    default: //Evento rejeitado
                        //Executar as ações necessárias
                        break;
                }
            }
        }

        private void bt_enviar_nfe_download_xml_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnviNFe
            {
                Versao = "4.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,
                NFe = new List<XmlNFe.NFe>
                {
                    new XmlNFe.NFe
                    {
                        InfNFe = new List<XmlNFe.InfNFe>
                        {
                            new XmlNFe.InfNFe
                            {
                                Versao = "4.00",
                                Ide = new XmlNFe.Ide
                                {
                                    CUF = UFBrasil.PR,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57990,
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
                                Emit = new XmlNFe.Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new XmlNFe.EnderEmit
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
                                Dest = new XmlNFe.Dest
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlNFe.EnderDest
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
                                AutXML = new List<XmlNFe.AutXML>
                                {
                                    new XmlNFe.AutXML
                                    {
                                        CNPJ = "00000000000000",
                                    },
                                    new XmlNFe.AutXML
                                    {
                                        CPF = "00000000000"
                                    },
                                    new XmlNFe.AutXML
                                    {
                                        CNPJ = "11111111111111"
                                    }
                                },
                                Det = CriarDet(),
                                Total = new XmlNFe.Total
                                {
                                    ICMSTot = new XmlNFe.ICMSTot
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
                                Transp = new XmlNFe.Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<XmlNFe.Vol>
                                    {
                                        new XmlNFe.Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new XmlNFe.Cobr()
                                {
                                    Fat = new XmlNFe.Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<XmlNFe.Dup>
                                    {
                                        new XmlNFe.Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new XmlNFe.Pag
                                {
                                    DetPag = new List<XmlNFe.DetPag>
                                    {
                                        new XmlNFe.DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 80.90
                                        }
                                    }
                                },
                                InfAdic = new XmlNFe.InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new XmlNFe.InfRespTec
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

            var autorizacao = new ServicoNFe.Autorizacao(xml, configuracao);
            var xmlString = autorizacao.ConteudoXMLAssinado.OuterXml;
            //Gravo no meu banco de dados o xmlString

            autorizacao.Executar();

            //Gravar o arquivo do conteúdo retornado em uma pasta qualquer para ter em segurança. Pode-se também gravar na base de dados. Fica a critério de cada um.
            File.WriteAllText(@"C:\testenfe\retorno\arquivoretorno.xml", autorizacao.RetornoWSString);

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
                        autorizacao.GravarXmlDistribuicao(@"C:\testenfe\");
                        var docProcNFe = autorizacao.NfeProcResult.GerarXML(); //Gerar o Objeto para pegar a string e gravar em banco de dados
                        MessageBox.Show(autorizacao.NfeProcResult.NomeArquivoDistribuicao);
                        break;

                    default:
                        //NF Rejeitada
                        break;
                }
            }
        }

        private void bt_distribuicao_dfe_versao_Click(object sender, EventArgs e)
        {
            var nsu = "000000000000000"; //Começar com o NSU 0 quando não tem o ultNSU

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                CertificadoDigital = CertificadoSelecionado
            };

            pb_consulta_dfe.Visible = true;
            pb_consulta_dfe.Minimum = 0;
            Application.DoEvents();
            pb_consulta_dfe.Refresh();

            while (true)
            {
                var xml = new XmlNFe.DistDFeInt
                {
                    Versao = "1.35",
                    TpAmb = TipoAmbiente.Homologacao,
                    CNPJ = "06117473000150",
                    CUFAutor = UFBrasil.PR,
                    DistNSU = new XmlNFe.DistNSU
                    {
                        UltNSU = nsu
                    }
                };

                var distribuicaoDFe = new ServicoNFe.DistribuicaoDFe(xml, configuracao);
                distribuicaoDFe.Executar();

                #region Atualizar ProgressBar

                if (pb_consulta_dfe.Maximum != Convert.ToInt32(distribuicaoDFe.Result.MaxNSU))
                {
                    pb_consulta_dfe.Maximum = Convert.ToInt32(distribuicaoDFe.Result.MaxNSU);
                }

                pb_consulta_dfe.Value = Convert.ToInt32(distribuicaoDFe.Result.UltNSU);
                pb_consulta_dfe.Refresh();
                Application.DoEvents();

                #endregion Atualizar ProgressBar

                if (distribuicaoDFe.Result.CStat == 138) // Documentos localizados e 137 = Não tem documentos
                {
                    var folder = @"C:\testenfe\doczip";

                    //Salvar XMLs do docZIP no HD
                    distribuicaoDFe.GravarXMLDocZIP(folder, true);
                }

                nsu = distribuicaoDFe.Result.UltNSU; //Salvar o ultNSU para usar na próxima consulta
                //Importante salvar o conteúdo de "nsu" na base de dados.

                if (Convert.ToInt64(distribuicaoDFe.Result.UltNSU) >= Convert.ToInt64(distribuicaoDFe.Result.MaxNSU))
                {
                    break;
                }

                pb_consulta_dfe.Visible = false;
                Application.DoEvents();

                //Guarde o conteúdo da variável "nsu" (que tem o ultNSU retornado) em sua base para ser utilizado na próxima consulta.
                //Agora aguarde 1 hora para dar sequencia na consulta a partir do ultNSU retornado, esta é a regra para evitar Consumo indevido.
            }
        }

        private void bt_distribuicao_dfe_cte_Click(object sender, EventArgs e)
        {
            var nsu = "000000000000000"; //Começar com o NSU 0 quando não tem o ultNSU			
            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.CTe,
                CertificadoDigital = CertificadoSelecionado
            };

            while (true)
            {
                var xml = new XmlCTe.DistDFeInt
                {
                    Versao = "1.00",
                    TpAmb = TipoAmbiente.Producao,
                    CNPJ = "06117473000150",
                    CUFAutor = UFBrasil.PR,
                    DistNSU = new XmlCTe.DistNSU
                    {
                        UltNSU = nsu
                    }
                };

                var distribuicaoDFe = new ServicoCTe.DistribuicaoDFe(xml, configuracao);
                distribuicaoDFe.Executar();


                if (distribuicaoDFe.Result.CStat.Equals(138)) //138 = Documentos localizados e 137 = Não tem documentos
                {
                    var folder = @"C:\testenfe\doczip";

                    //Salvar os XMLs do docZIP no HD
                    distribuicaoDFe.GravarXMLDocZIP(folder);
                }

                nsu = distribuicaoDFe.Result.UltNSU; //Salvar o ultNSU para usar na próxima consulta

                if (Convert.ToInt64(distribuicaoDFe.Result.UltNSU) >= Convert.ToInt64(distribuicaoDFe.Result.MaxNSU))
                {
                    break;
                }
            }

            //Guarde o conteúdo da variável "nsu" (que tem o ultNSU retornado) em sua base para ser utilizado na próxima consulta.
            //Agora aguarde 1 hora para dar sequencia na consulta a partir do ultNSU retornado, esta é a regra para evitar Consumo indevido.      
        }

        private void bt_validar_xml_dll_Click(object sender, EventArgs e)
        {
            try
            {
                var validarSchema = new DFe.ValidarSchema();
                var doc = new XmlDocument();

                //Validar XML da ProcNFe - Sem falhas
                doc.Load(@"C:\Projetos\UniNFe\exemplos\NFe e NFCe 4.00\NFe\41170706117473000150550010000463191912756548-procNFe.xml");

                var schema = "NFe.procNFe_v4.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/nfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso");
                }

                //Validar XML da ProcNFe - com falhas
                doc.Load(@"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml");

                schema = "NFe.procNFe_v4.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/nfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }

                //Validar XML da NFe
                doc.Load(@"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml");

                schema = "NFe.nfe_v4.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/nfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }

                //Validar uma NFCe                
                doc.Load(@"C:\ProjetoLivesDLL\recursos\41170706117473000150550010000463201612756527-nfe.xml");

                schema = "NFe.nfe_v4.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/nfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }

                //Validar XML de consulta status da NFe
                doc.Load(@"D:\testenfe\20100222T222310-ped-sta.xml");

                schema = "NFe.consStatServ_v4.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/nfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }

                //Validar XML de CTe                
                doc.Load(@"D:\testenfe\51160624686092000173570010000000031000000020-cte.XML");

                schema = "CTe.cte_v3.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/cte");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }

                //Validar XML de consulta status da MDFe
                doc.Load(@"D:\testenfe\20170210T100210-ped-sta.xml");

                schema = "MDFe.consStatServMDFe_v3.00.xsd";
                validarSchema.Validar(doc, schema, "http://www.portalfiscal.inf.br/mdfe");

                if (!validarSchema.Success)
                {
                    MessageBox.Show("Code: " + validarSchema.ErrorCode + "\r\n\r\nMessage: " + validarSchema.ErrorMessage);
                }
                else
                {
                    MessageBox.Show("XML validado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bt_tratamento_excecoes_dll_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.ConsStatServ
            {
                Versao = "3.00", //Mudar de 4.00 para 3.00
                CUF = UFBrasil.PR,
                TpAmb = TipoAmbiente.Homologacao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };

            try
            {
                var statusServico = new ServicoNFe.StatusServico(xml, configuracao);
                statusServico.Executar();

                MessageBox.Show(statusServico.Result.CStat + " " + statusServico.Result.XMotivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetLastException().Message);
            }
        }

        private void bt_tratamento_excecoes_dll_2_Click(object sender, EventArgs e)
        {
            var xml = new XmlNFe.EnviNFe
            {
                Versao = "3.00",
                IdLote = "000000000000001",
                IndSinc = SimNao.Sim,
                NFe = new List<XmlNFe.NFe>
                {
                    new XmlNFe.NFe
                    {
                        InfNFe = new List<XmlNFe.InfNFe>
                        {
                            new XmlNFe.InfNFe
                            {
                                Versao = "4.00",
                                Ide = new XmlNFe.Ide
                                {
                                    CUF = UFBrasil.MG,
                                    NatOp = "VENDA PRODUC.DO ESTABELEC",
                                    Mod = ModeloDFe.NFe,
                                    Serie = 1,
                                    NNF = 57990,
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
                                Emit = new XmlNFe.Emit
                                {
                                    CNPJ = "06117473000150",
                                    XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA",
                                    XFant = "UNIMAKE - PARANAVAI",
                                    EnderEmit = new XmlNFe.EnderEmit
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
                                Dest = new XmlNFe.Dest
                                {
                                    CNPJ = "04218457000128",
                                    XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                                    EnderDest = new XmlNFe.EnderDest
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
                                Det = CriarDet(),
                                Total = new XmlNFe.Total
                                {
                                    ICMSTot = new XmlNFe.ICMSTot
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
                                Transp = new XmlNFe.Transp
                                {
                                    ModFrete = ModalidadeFrete.ContratacaoFretePorContaRemetente_CIF,
                                    Vol = new List<XmlNFe.Vol>
                                    {
                                        new XmlNFe.Vol
                                        {
                                            QVol = 1,
                                            Esp = "LU",
                                            Marca = "UNIMAKE",
                                            PesoL = 0.000,
                                            PesoB = 0.000
                                        }
                                    }
                                },
                                Cobr = new XmlNFe.Cobr()
                                {
                                    Fat = new XmlNFe.Fat
                                    {
                                        NFat = "057910",
                                        VOrig = 84.90,
                                        VDesc = 0,
                                        VLiq = 84.90
                                    },
                                    Dup = new List<XmlNFe.Dup>
                                    {
                                        new XmlNFe.Dup
                                        {
                                            NDup = "001",
                                            DVenc = DateTime.Now,
                                            VDup = 84.90
                                        }
                                    }
                                },
                                Pag = new XmlNFe.Pag
                                {
                                    DetPag = new List<XmlNFe.DetPag>
                                    {
                                        new XmlNFe.DetPag
                                        {
                                            IndPag = IndicadorPagamento.PagamentoVista,
                                            TPag = MeioPagamento.Dinheiro,
                                            VPag = 80.90
                                        }
                                    }
                                },
                                InfAdic = new XmlNFe.InfAdic
                                {
                                    InfCpl = ";CONTROLE: 0000241197;PEDIDO(S) ATENDIDO(S): 300474;Empresa optante pelo simples nacional, conforme lei compl. 128 de 19/12/2008;Permite o aproveitamento do credito de ICMS no valor de R$ 2,40, correspondente ao percentual de 2,83% . Nos termos do Art. 23 - LC 123/2006 (Resolucoes CGSN n. 10/2007 e 53/2008);Voce pagou aproximadamente: R$ 6,69 trib. federais / R$ 5,94 trib. estaduais / R$ 0,00 trib. municipais. Fonte: IBPT/empresometro.com.br 18.2.B A3S28F;",
                                },
                                InfRespTec = new XmlNFe.InfRespTec
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

            try
            {
                var autorizacao = new ServicoNFe.Autorizacao(xml, configuracao);
                autorizacao.Executar();
            }
            catch (ValidarXMLException ex)
            {
                MessageBox.Show(ex.GetLastException().Message);
            }
            catch (CertificadoDigitalException ex)
            {
                MessageBox.Show(ex.GetLastException().Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetLastException().Message);
            }
        }

        private void bt_resgatando_info_certificado_Click(object sender, EventArgs e)
        {
            var certificadoDigital = new CertificadoDigital();

            var certSel = certificadoDigital.AbrirTelaSelecao();

            string infoCertificado = "Data validade inicial: " + certSel.NotBefore + "\r\n\r\n" +
                "Data validade final: " + certSel.NotAfter + "\r\n\r\n" +
                "Sujeito/Pessoa: " + certSel.Subject + "\r\n\r\n" +
                "Emissor: " + certSel.Issuer + "\r\n\r\n" +
                "Serial Number: " + certSel.SerialNumber + "\r\n\r\n" +
                "Thumbprint: " + certSel.Thumbprint;

            MessageBox.Show(infoCertificado);
        }

        private void bt_teste_internet_Click(object sender, EventArgs e)
        {
            //Testando a internet sem considerar servidor de proxy
            if (!Unimake.Net.Utility.HasInternetConnection())
            {
                MessageBox.Show("Sem internet!!!");
            }
            else
            {
                MessageBox.Show("Internet ok");
            }

            //Testando a internet considerando que tem um servidor proxy na rede
            var proxy = Unimake.Net.Utility.GetProxy("192.168.1.111", "UserTest", "PassTest", 123); //Passando o IP do servidor de proxy
            if (!Unimake.Net.Utility.HasInternetConnection(proxy))
            {
                MessageBox.Show("Sem internet!!!");
            }
            else
            {
                MessageBox.Show("Internet ok");
            }

            var proxy2 = Unimake.Net.Utility.GetProxy("", "UserTest", "PassTest", 123, true); //Detectando servidor automaticamente
            if (!Unimake.Net.Utility.HasInternetConnection(proxy2))
            {
                MessageBox.Show("Sem internet!!!");
            }
            else
            {
                MessageBox.Show("Internet ok");
            }
        }

        //Fim Serviços Diversos

    }
}
