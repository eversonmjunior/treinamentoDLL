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
        private void bt_imprimir_danfe_Click(object sender, EventArgs e)
        {
            var config = new DANFe.Configurations.UnidanfeConfiguration
            {
                Arquivo = @"F:\adm\NFe\Emp0001_Fil002_Envio\Enviadas\Autorizados\202202\41220206117473000150550010000717301715751703-procNFe.xml",
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

            var arqXMLNFCe = Path.Combine(@"D:\testenfe\NFCeOffline", autorizacao.EnviNFe.NFe[0].InfNFe[0].Chave + "-nfe.xml");

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
                        recepcaoEvento.GravarXmlDistribuicao(@"d:\testenfe");
                        break;

                    default:
                        break;
                        //Tratamentos caso haja rejeição
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

            if(autorizacaoSinc.Result.ProtMDFe.InfProt.CStat == 100)
            {
                MessageBox.Show(autorizacaoSinc.Result.ProtMDFe.InfProt.NProt);
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
                    recepcaoEvento.GravarXmlDistribuicao(@"d:\testenfe");
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

        //Fim Serviços MDFe

        //Serviços NFSe
        private void bt_cancelar_nfse_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\CancelarNfseEnvio-ped-cannfse.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.MDFe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeCancelarNfse,
                SchemaVersao = "2.02",
                CodigoMunicipio = 5107909
            };

            var cancelarNFSe =  new ServicoNFSe.CancelarNfse(xmlDoc, configuracao);
            cancelarNFSe.Executar();

            //var teste = new ServicoNFSe.CancelamentoNfe(xmlDoc, configuracao); -> todos os serviços NFSe

            MessageBox.Show(cancelarNFSe.RetornoWSString);
        }

        private void bt_env_rps_sinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\GerarNfseEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe, 
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeGerarNfse,
                SchemaVersao = "2.04",
                CodigoMunicipio = 2933307
            };

            var gerarNfse = new ServicoNFSe.GerarNfse(xmlDoc, configuracao);
            gerarNfse.Executar();

            MessageBox.Show(gerarNfse.RetornoWSString);
        }

        private void bt_env_lote_rps_sinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\EnviarLoteRpsSincronoEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeRecepcionarLoteRpsSincrono,
                SchemaVersao = "2.04",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 2933307
            };

            var recepcionarLoteRpsSincrono = new ServicoNFSe.RecepcionarLoteRpsSincrono(xmlDoc, configuracao);
            recepcionarLoteRpsSincrono.Executar();

            MessageBox.Show(recepcionarLoteRpsSincrono.RetornoWSString);
        }

        private void bt_env_lote_rps_assinc_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\EnviarLoteRpsEnvio-env-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeRecepcionarLoteRps,
                SchemaVersao = "2.04",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 2933307
            };

            var recepcionarLoteRps = new ServicoNFSe.RecepcionarLoteRps(xmlDoc, configuracao);
            recepcionarLoteRps.Executar();

            MessageBox.Show(recepcionarLoteRps.RetornoWSString);
        }

        private void bt_consultar_lote_rps_Click(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\ConsultarLoteRpsEnvio-ped-loterps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeConsultarLoteRps,
                SchemaVersao = "2.04",
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
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\ConsultarNfseRpsEnvio-ped-sitnfserps.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe, 
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeConsultarNfsePorRps,
                SchemaVersao = "2.04",
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
            xmlDoc.Load(@"D:\Wandrey\OneDrive\Documentos\Unimake\Treinamentos\LIVEs UniNFe\TreinamentoDLL\Recursos\SubstituirNfseEnvio-ped-substnfse.xml");

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFSe,
                CertificadoDigital = CertificadoSelecionado,
                Servico = Servico.NFSeSubstituirNfse,
                SchemaVersao = "2.04",
                TipoAmbiente = TipoAmbiente.Producao,
                CodigoMunicipio = 2933307
            };

            var substituirNFse = new ServicoNFSe.SubstituirNfse(xmlDoc, configuracao);
            substituirNFse.Executar();

            MessageBox.Show(substituirNFse.RetornoWSString);
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


        //Fim Serviços CTe e CTeOs
    }
}
