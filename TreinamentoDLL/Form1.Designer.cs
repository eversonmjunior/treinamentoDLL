namespace TreinamentoDLL
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bt_consulta_status = new System.Windows.Forms.Button();
            this.bt_consulta_situacao_NFe = new System.Windows.Forms.Button();
            this.bt_env_nfe_sinc = new System.Windows.Forms.Button();
            this.bt_env_nfe_assinc = new System.Windows.Forms.Button();
            this.bt_env_nfe_assinc_lote = new System.Windows.Forms.Button();
            this.bt_inutilizacao = new System.Windows.Forms.Button();
            this.bt_cons_cadas_contribuinte = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_env_nfe_desserializacao = new System.Windows.Forms.Button();
            this.bt_env_evento_cancel_nfe = new System.Windows.Forms.Button();
            this.bt_env_evento_cce = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_consulta_status_nfce = new System.Windows.Forms.Button();
            this.bt_consulta_situacao_nfce = new System.Windows.Forms.Button();
            this.bt_env_nfce_sinc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.bt_env_mdfe_assinc = new System.Windows.Forms.Button();
            this.bt_consulta_situacao_mdfe = new System.Windows.Forms.Button();
            this.bt_consulta_status_mdfe = new System.Windows.Forms.Button();
            this.bt_env_mdfe_sinc = new System.Windows.Forms.Button();
            this.bt_imprimir_danfe = new System.Windows.Forms.Button();
            this.bt_exe_config_danfe = new System.Windows.Forms.Button();
            this.bt_env_nfce_contingencia_offline = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_env_lote_rps_assinc = new System.Windows.Forms.Button();
            this.bt_env_lote_rps_sinc = new System.Windows.Forms.Button();
            this.bt_env_rps_sinc = new System.Windows.Forms.Button();
            this.bt_cancelar_nfse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.bt_env_evento_canc_substituicao = new System.Windows.Forms.Button();
            this.bt_gerar_nfce_contingencia = new System.Windows.Forms.Button();
            this.bt_resgatar_xml_da_nfce = new System.Windows.Forms.Button();
            this.bt_enviar_evento_cancelamento = new System.Windows.Forms.Button();
            this.bt_consultar_nfse_por_rps = new System.Windows.Forms.Button();
            this.bt_consultar_lote_rps = new System.Windows.Forms.Button();
            this.bt_enviar_evento_encerramento = new System.Windows.Forms.Button();
            this.bt_consultar_mdfe_nao_encerrados = new System.Windows.Forms.Button();
            this.bt_substituir_nfse = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.bt_consulta_status_cte = new System.Windows.Forms.Button();
            this.bt_enviar_cte_assinc_lote = new System.Windows.Forms.Button();
            this.bt_enviar_cteos_sinc = new System.Windows.Forms.Button();
            this.bt_enviar_evento_epec = new System.Windows.Forms.Button();
            this.bt_recuperar_xml_nfe_distribuicao = new System.Windows.Forms.Button();
            this.bt_recuperar_xml_nfe_distribuicao_2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.bt_imprimir_danfe_etiqueta = new System.Windows.Forms.Button();
            this.bt_carregar_cert_a3 = new System.Windows.Forms.Button();
            this.bt_evento_canc_cte = new System.Windows.Forms.Button();
            this.bt_evento_canc_cteos = new System.Windows.Forms.Button();
            this.bt_evento_cce_e_cte = new System.Windows.Forms.Button();
            this.bt_evento_cce_e_cteos = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "NFe";
            // 
            // bt_consulta_status
            // 
            this.bt_consulta_status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_consulta_status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_status.Location = new System.Drawing.Point(12, 25);
            this.bt_consulta_status.Name = "bt_consulta_status";
            this.bt_consulta_status.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_status.TabIndex = 1;
            this.bt_consulta_status.Text = "Consulta Status";
            this.bt_consulta_status.UseVisualStyleBackColor = false;
            this.bt_consulta_status.Click += new System.EventHandler(this.bt_consulta_status_Click);
            // 
            // bt_consulta_situacao_NFe
            // 
            this.bt_consulta_situacao_NFe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_consulta_situacao_NFe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_situacao_NFe.Location = new System.Drawing.Point(12, 54);
            this.bt_consulta_situacao_NFe.Name = "bt_consulta_situacao_NFe";
            this.bt_consulta_situacao_NFe.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_situacao_NFe.TabIndex = 2;
            this.bt_consulta_situacao_NFe.Text = "Consulta Situação";
            this.bt_consulta_situacao_NFe.UseVisualStyleBackColor = false;
            this.bt_consulta_situacao_NFe.Click += new System.EventHandler(this.bt_consulta_situacao_NFe_Click);
            // 
            // bt_env_nfe_sinc
            // 
            this.bt_env_nfe_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_nfe_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfe_sinc.Location = new System.Drawing.Point(12, 117);
            this.bt_env_nfe_sinc.Name = "bt_env_nfe_sinc";
            this.bt_env_nfe_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_nfe_sinc.TabIndex = 3;
            this.bt_env_nfe_sinc.Text = "Enviar NFe Síncrono";
            this.bt_env_nfe_sinc.UseVisualStyleBackColor = false;
            this.bt_env_nfe_sinc.Click += new System.EventHandler(this.bt_env_nfe_sinc_Click);
            // 
            // bt_env_nfe_assinc
            // 
            this.bt_env_nfe_assinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_nfe_assinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfe_assinc.Location = new System.Drawing.Point(12, 146);
            this.bt_env_nfe_assinc.Name = "bt_env_nfe_assinc";
            this.bt_env_nfe_assinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_nfe_assinc.TabIndex = 4;
            this.bt_env_nfe_assinc.Text = "Enviar NFe Assíncrono";
            this.bt_env_nfe_assinc.UseVisualStyleBackColor = false;
            this.bt_env_nfe_assinc.Click += new System.EventHandler(this.bt_env_nfe_assinc_Click);
            // 
            // bt_env_nfe_assinc_lote
            // 
            this.bt_env_nfe_assinc_lote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_nfe_assinc_lote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfe_assinc_lote.Location = new System.Drawing.Point(12, 175);
            this.bt_env_nfe_assinc_lote.Name = "bt_env_nfe_assinc_lote";
            this.bt_env_nfe_assinc_lote.Size = new System.Drawing.Size(180, 23);
            this.bt_env_nfe_assinc_lote.TabIndex = 5;
            this.bt_env_nfe_assinc_lote.Text = "Enviar NFe Assíncrono em Lote";
            this.bt_env_nfe_assinc_lote.UseVisualStyleBackColor = false;
            this.bt_env_nfe_assinc_lote.Click += new System.EventHandler(this.bt_env_nfe_assinc_lote_Click);
            // 
            // bt_inutilizacao
            // 
            this.bt_inutilizacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_inutilizacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_inutilizacao.Location = new System.Drawing.Point(12, 204);
            this.bt_inutilizacao.Name = "bt_inutilizacao";
            this.bt_inutilizacao.Size = new System.Drawing.Size(180, 23);
            this.bt_inutilizacao.TabIndex = 6;
            this.bt_inutilizacao.Text = "Inutilização";
            this.bt_inutilizacao.UseVisualStyleBackColor = false;
            this.bt_inutilizacao.Click += new System.EventHandler(this.bt_inutilizacao_Click);
            // 
            // bt_cons_cadas_contribuinte
            // 
            this.bt_cons_cadas_contribuinte.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_cons_cadas_contribuinte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_cons_cadas_contribuinte.Location = new System.Drawing.Point(12, 236);
            this.bt_cons_cadas_contribuinte.Name = "bt_cons_cadas_contribuinte";
            this.bt_cons_cadas_contribuinte.Size = new System.Drawing.Size(180, 23);
            this.bt_cons_cadas_contribuinte.TabIndex = 7;
            this.bt_cons_cadas_contribuinte.Text = "Consulta Cadastro Contribuinte";
            this.bt_cons_cadas_contribuinte.UseVisualStyleBackColor = false;
            this.bt_cons_cadas_contribuinte.Click += new System.EventHandler(this.bt_cons_cadas_contribuinte_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "*Alguns botões não funcionam";
            // 
            // bt_env_nfe_desserializacao
            // 
            this.bt_env_nfe_desserializacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_nfe_desserializacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfe_desserializacao.Location = new System.Drawing.Point(12, 265);
            this.bt_env_nfe_desserializacao.Name = "bt_env_nfe_desserializacao";
            this.bt_env_nfe_desserializacao.Size = new System.Drawing.Size(180, 37);
            this.bt_env_nfe_desserializacao.TabIndex = 9;
            this.bt_env_nfe_desserializacao.Text = "Enviar NFe com Desserialização do XML";
            this.bt_env_nfe_desserializacao.UseVisualStyleBackColor = false;
            this.bt_env_nfe_desserializacao.Click += new System.EventHandler(this.bt_env_nfe_desserializacao_Click);
            // 
            // bt_env_evento_cancel_nfe
            // 
            this.bt_env_evento_cancel_nfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_evento_cancel_nfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_evento_cancel_nfe.Location = new System.Drawing.Point(12, 308);
            this.bt_env_evento_cancel_nfe.Name = "bt_env_evento_cancel_nfe";
            this.bt_env_evento_cancel_nfe.Size = new System.Drawing.Size(180, 23);
            this.bt_env_evento_cancel_nfe.TabIndex = 10;
            this.bt_env_evento_cancel_nfe.Text = "Enviar Evento de Cancelamento";
            this.bt_env_evento_cancel_nfe.UseVisualStyleBackColor = false;
            this.bt_env_evento_cancel_nfe.Click += new System.EventHandler(this.bt_env_evento_cancel_nfe_Click);
            // 
            // bt_env_evento_cce
            // 
            this.bt_env_evento_cce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_evento_cce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_evento_cce.Location = new System.Drawing.Point(12, 337);
            this.bt_env_evento_cce.Name = "bt_env_evento_cce";
            this.bt_env_evento_cce.Size = new System.Drawing.Size(180, 23);
            this.bt_env_evento_cce.TabIndex = 11;
            this.bt_env_evento_cce.Text = "Enviar Evento de CCe";
            this.bt_env_evento_cce.UseVisualStyleBackColor = false;
            this.bt_env_evento_cce.Click += new System.EventHandler(this.bt_env_evento_cce_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "NFCe";
            // 
            // bt_consulta_status_nfce
            // 
            this.bt_consulta_status_nfce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_consulta_status_nfce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_status_nfce.Location = new System.Drawing.Point(225, 25);
            this.bt_consulta_status_nfce.Name = "bt_consulta_status_nfce";
            this.bt_consulta_status_nfce.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_status_nfce.TabIndex = 13;
            this.bt_consulta_status_nfce.Text = "Consulta Status";
            this.bt_consulta_status_nfce.UseVisualStyleBackColor = false;
            this.bt_consulta_status_nfce.Click += new System.EventHandler(this.bt_consulta_status_nfce_Click);
            // 
            // bt_consulta_situacao_nfce
            // 
            this.bt_consulta_situacao_nfce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_consulta_situacao_nfce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_situacao_nfce.Location = new System.Drawing.Point(225, 54);
            this.bt_consulta_situacao_nfce.Name = "bt_consulta_situacao_nfce";
            this.bt_consulta_situacao_nfce.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_situacao_nfce.TabIndex = 14;
            this.bt_consulta_situacao_nfce.Text = "Consulta Situação";
            this.bt_consulta_situacao_nfce.UseVisualStyleBackColor = false;
            this.bt_consulta_situacao_nfce.Click += new System.EventHandler(this.bt_consulta_situacao_nfce_Click);
            // 
            // bt_env_nfce_sinc
            // 
            this.bt_env_nfce_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_env_nfce_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfce_sinc.Location = new System.Drawing.Point(225, 83);
            this.bt_env_nfce_sinc.Name = "bt_env_nfce_sinc";
            this.bt_env_nfce_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_nfce_sinc.TabIndex = 15;
            this.bt_env_nfce_sinc.Text = "Enviar NFCe Síncrono";
            this.bt_env_nfce_sinc.UseVisualStyleBackColor = false;
            this.bt_env_nfce_sinc.Click += new System.EventHandler(this.bt_env_nfce_sinc_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(429, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "MDFe";
            // 
            // bt_env_mdfe_assinc
            // 
            this.bt_env_mdfe_assinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_env_mdfe_assinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_mdfe_assinc.Location = new System.Drawing.Point(432, 83);
            this.bt_env_mdfe_assinc.Name = "bt_env_mdfe_assinc";
            this.bt_env_mdfe_assinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_mdfe_assinc.TabIndex = 19;
            this.bt_env_mdfe_assinc.Text = "Enviar MDFe Assíncrono";
            this.bt_env_mdfe_assinc.UseVisualStyleBackColor = false;
            this.bt_env_mdfe_assinc.Click += new System.EventHandler(this.bt_env_mdfe_assinc_Click);
            // 
            // bt_consulta_situacao_mdfe
            // 
            this.bt_consulta_situacao_mdfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_consulta_situacao_mdfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_situacao_mdfe.Location = new System.Drawing.Point(432, 54);
            this.bt_consulta_situacao_mdfe.Name = "bt_consulta_situacao_mdfe";
            this.bt_consulta_situacao_mdfe.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_situacao_mdfe.TabIndex = 18;
            this.bt_consulta_situacao_mdfe.Text = "Consulta Situação";
            this.bt_consulta_situacao_mdfe.UseVisualStyleBackColor = false;
            this.bt_consulta_situacao_mdfe.Click += new System.EventHandler(this.bt_consulta_situacao_mdfe_Click);
            // 
            // bt_consulta_status_mdfe
            // 
            this.bt_consulta_status_mdfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_consulta_status_mdfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_status_mdfe.Location = new System.Drawing.Point(432, 25);
            this.bt_consulta_status_mdfe.Name = "bt_consulta_status_mdfe";
            this.bt_consulta_status_mdfe.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_status_mdfe.TabIndex = 17;
            this.bt_consulta_status_mdfe.Text = "Consulta Status";
            this.bt_consulta_status_mdfe.UseVisualStyleBackColor = false;
            this.bt_consulta_status_mdfe.Click += new System.EventHandler(this.bt_consulta_status_mdfe_Click);
            // 
            // bt_env_mdfe_sinc
            // 
            this.bt_env_mdfe_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_env_mdfe_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_mdfe_sinc.Location = new System.Drawing.Point(432, 112);
            this.bt_env_mdfe_sinc.Name = "bt_env_mdfe_sinc";
            this.bt_env_mdfe_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_mdfe_sinc.TabIndex = 20;
            this.bt_env_mdfe_sinc.Text = "Enviar MDFe Síncrono";
            this.bt_env_mdfe_sinc.UseVisualStyleBackColor = false;
            this.bt_env_mdfe_sinc.Click += new System.EventHandler(this.bt_env_mdfe_sinc_Click);
            // 
            // bt_imprimir_danfe
            // 
            this.bt_imprimir_danfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_imprimir_danfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_imprimir_danfe.Location = new System.Drawing.Point(12, 366);
            this.bt_imprimir_danfe.Name = "bt_imprimir_danfe";
            this.bt_imprimir_danfe.Size = new System.Drawing.Size(180, 23);
            this.bt_imprimir_danfe.TabIndex = 21;
            this.bt_imprimir_danfe.Text = "Imprimir DANFe";
            this.bt_imprimir_danfe.UseVisualStyleBackColor = false;
            this.bt_imprimir_danfe.Click += new System.EventHandler(this.bt_imprimir_danfe_Click);
            // 
            // bt_exe_config_danfe
            // 
            this.bt_exe_config_danfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_exe_config_danfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_exe_config_danfe.Location = new System.Drawing.Point(12, 395);
            this.bt_exe_config_danfe.Name = "bt_exe_config_danfe";
            this.bt_exe_config_danfe.Size = new System.Drawing.Size(180, 23);
            this.bt_exe_config_danfe.TabIndex = 22;
            this.bt_exe_config_danfe.Text = "Executar tela config DANFe";
            this.bt_exe_config_danfe.UseVisualStyleBackColor = false;
            this.bt_exe_config_danfe.Click += new System.EventHandler(this.bt_exe_config_danfe_Click);
            // 
            // bt_env_nfce_contingencia_offline
            // 
            this.bt_env_nfce_contingencia_offline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_env_nfce_contingencia_offline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_nfce_contingencia_offline.Location = new System.Drawing.Point(225, 141);
            this.bt_env_nfce_contingencia_offline.Name = "bt_env_nfce_contingencia_offline";
            this.bt_env_nfce_contingencia_offline.Size = new System.Drawing.Size(180, 23);
            this.bt_env_nfce_contingencia_offline.TabIndex = 23;
            this.bt_env_nfce_contingencia_offline.Text = "Enviar NFCe Contingência Offline";
            this.bt_env_nfce_contingencia_offline.UseVisualStyleBackColor = false;
            this.bt_env_nfce_contingencia_offline.Click += new System.EventHandler(this.bt_env_nfce_contingencia_offline_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "*XML fictício";
            // 
            // bt_env_lote_rps_assinc
            // 
            this.bt_env_lote_rps_assinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_lote_rps_assinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_lote_rps_assinc.Location = new System.Drawing.Point(636, 55);
            this.bt_env_lote_rps_assinc.Name = "bt_env_lote_rps_assinc";
            this.bt_env_lote_rps_assinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_lote_rps_assinc.TabIndex = 29;
            this.bt_env_lote_rps_assinc.Text = "Envio Lote RPS Assíncrono";
            this.bt_env_lote_rps_assinc.UseVisualStyleBackColor = false;
            this.bt_env_lote_rps_assinc.Click += new System.EventHandler(this.bt_env_lote_rps_assinc_Click);
            // 
            // bt_env_lote_rps_sinc
            // 
            this.bt_env_lote_rps_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_lote_rps_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_lote_rps_sinc.Location = new System.Drawing.Point(636, 84);
            this.bt_env_lote_rps_sinc.Name = "bt_env_lote_rps_sinc";
            this.bt_env_lote_rps_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_lote_rps_sinc.TabIndex = 28;
            this.bt_env_lote_rps_sinc.Text = "Envio Lote RPS Síncrono";
            this.bt_env_lote_rps_sinc.UseVisualStyleBackColor = false;
            this.bt_env_lote_rps_sinc.Click += new System.EventHandler(this.bt_env_lote_rps_sinc_Click);
            // 
            // bt_env_rps_sinc
            // 
            this.bt_env_rps_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_env_rps_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_rps_sinc.Location = new System.Drawing.Point(636, 113);
            this.bt_env_rps_sinc.Name = "bt_env_rps_sinc";
            this.bt_env_rps_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_env_rps_sinc.TabIndex = 27;
            this.bt_env_rps_sinc.Text = "Envio RPS Síncrono";
            this.bt_env_rps_sinc.UseVisualStyleBackColor = false;
            this.bt_env_rps_sinc.Click += new System.EventHandler(this.bt_env_rps_sinc_Click);
            // 
            // bt_cancelar_nfse
            // 
            this.bt_cancelar_nfse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_cancelar_nfse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_cancelar_nfse.Location = new System.Drawing.Point(636, 25);
            this.bt_cancelar_nfse.Name = "bt_cancelar_nfse";
            this.bt_cancelar_nfse.Size = new System.Drawing.Size(180, 23);
            this.bt_cancelar_nfse.TabIndex = 26;
            this.bt_cancelar_nfse.Text = "Cancelar NFSe";
            this.bt_cancelar_nfse.UseVisualStyleBackColor = false;
            this.bt_cancelar_nfse.Click += new System.EventHandler(this.bt_cancelar_nfse_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(633, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "NFSe";
            // 
            // bt_env_evento_canc_substituicao
            // 
            this.bt_env_evento_canc_substituicao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_env_evento_canc_substituicao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_env_evento_canc_substituicao.Location = new System.Drawing.Point(225, 170);
            this.bt_env_evento_canc_substituicao.Name = "bt_env_evento_canc_substituicao";
            this.bt_env_evento_canc_substituicao.Size = new System.Drawing.Size(180, 38);
            this.bt_env_evento_canc_substituicao.TabIndex = 30;
            this.bt_env_evento_canc_substituicao.Text = "Enviar Evento Cancelamento por Substituição";
            this.bt_env_evento_canc_substituicao.UseVisualStyleBackColor = false;
            this.bt_env_evento_canc_substituicao.Click += new System.EventHandler(this.bt_env_evento_canc_substituicao_Click);
            // 
            // bt_gerar_nfce_contingencia
            // 
            this.bt_gerar_nfce_contingencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_gerar_nfce_contingencia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_gerar_nfce_contingencia.Location = new System.Drawing.Point(225, 112);
            this.bt_gerar_nfce_contingencia.Name = "bt_gerar_nfce_contingencia";
            this.bt_gerar_nfce_contingencia.Size = new System.Drawing.Size(180, 23);
            this.bt_gerar_nfce_contingencia.TabIndex = 31;
            this.bt_gerar_nfce_contingencia.Text = "Gerar NFCe Contingência";
            this.bt_gerar_nfce_contingencia.UseVisualStyleBackColor = false;
            this.bt_gerar_nfce_contingencia.Click += new System.EventHandler(this.bt_gerar_nfce_contingencia_Click);
            // 
            // bt_resgatar_xml_da_nfce
            // 
            this.bt_resgatar_xml_da_nfce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.bt_resgatar_xml_da_nfce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_resgatar_xml_da_nfce.Location = new System.Drawing.Point(225, 277);
            this.bt_resgatar_xml_da_nfce.Name = "bt_resgatar_xml_da_nfce";
            this.bt_resgatar_xml_da_nfce.Size = new System.Drawing.Size(180, 38);
            this.bt_resgatar_xml_da_nfce.TabIndex = 32;
            this.bt_resgatar_xml_da_nfce.Text = "Resgatar o XML da NFCe, sem as tags de lote";
            this.bt_resgatar_xml_da_nfce.UseVisualStyleBackColor = false;
            // 
            // bt_enviar_evento_cancelamento
            // 
            this.bt_enviar_evento_cancelamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_enviar_evento_cancelamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_enviar_evento_cancelamento.Location = new System.Drawing.Point(432, 141);
            this.bt_enviar_evento_cancelamento.Name = "bt_enviar_evento_cancelamento";
            this.bt_enviar_evento_cancelamento.Size = new System.Drawing.Size(180, 23);
            this.bt_enviar_evento_cancelamento.TabIndex = 34;
            this.bt_enviar_evento_cancelamento.Text = "Enviar Evento de Cancelamento";
            this.bt_enviar_evento_cancelamento.UseVisualStyleBackColor = false;
            this.bt_enviar_evento_cancelamento.Click += new System.EventHandler(this.bt_enviar_evento_cancelamento_Click);
            // 
            // bt_consultar_nfse_por_rps
            // 
            this.bt_consultar_nfse_por_rps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_consultar_nfse_por_rps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consultar_nfse_por_rps.Location = new System.Drawing.Point(636, 277);
            this.bt_consultar_nfse_por_rps.Name = "bt_consultar_nfse_por_rps";
            this.bt_consultar_nfse_por_rps.Size = new System.Drawing.Size(180, 23);
            this.bt_consultar_nfse_por_rps.TabIndex = 35;
            this.bt_consultar_nfse_por_rps.Text = "Consultar NFSe por RPS";
            this.bt_consultar_nfse_por_rps.UseVisualStyleBackColor = false;
            this.bt_consultar_nfse_por_rps.Click += new System.EventHandler(this.bt_consultar_nfse_por_rps_Click);
            // 
            // bt_consultar_lote_rps
            // 
            this.bt_consultar_lote_rps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_consultar_lote_rps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consultar_lote_rps.Location = new System.Drawing.Point(636, 141);
            this.bt_consultar_lote_rps.Name = "bt_consultar_lote_rps";
            this.bt_consultar_lote_rps.Size = new System.Drawing.Size(180, 23);
            this.bt_consultar_lote_rps.TabIndex = 35;
            this.bt_consultar_lote_rps.Text = "Consultar Lote RPS";
            this.bt_consultar_lote_rps.UseVisualStyleBackColor = false;
            this.bt_consultar_lote_rps.Click += new System.EventHandler(this.bt_consultar_lote_rps_Click);
            // 
            // bt_enviar_evento_encerramento
            // 
            this.bt_enviar_evento_encerramento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_enviar_evento_encerramento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_enviar_evento_encerramento.Location = new System.Drawing.Point(432, 170);
            this.bt_enviar_evento_encerramento.Name = "bt_enviar_evento_encerramento";
            this.bt_enviar_evento_encerramento.Size = new System.Drawing.Size(180, 23);
            this.bt_enviar_evento_encerramento.TabIndex = 36;
            this.bt_enviar_evento_encerramento.Text = "Enviar Evento de Encerramento";
            this.bt_enviar_evento_encerramento.UseVisualStyleBackColor = false;
            this.bt_enviar_evento_encerramento.Click += new System.EventHandler(this.bt_enviar_evento_encerramento_Click);
            // 
            // bt_consultar_mdfe_nao_encerrados
            // 
            this.bt_consultar_mdfe_nao_encerrados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_consultar_mdfe_nao_encerrados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consultar_mdfe_nao_encerrados.Location = new System.Drawing.Point(432, 199);
            this.bt_consultar_mdfe_nao_encerrados.Name = "bt_consultar_mdfe_nao_encerrados";
            this.bt_consultar_mdfe_nao_encerrados.Size = new System.Drawing.Size(180, 23);
            this.bt_consultar_mdfe_nao_encerrados.TabIndex = 37;
            this.bt_consultar_mdfe_nao_encerrados.Text = "Consultar MDFe Não Encerrado";
            this.bt_consultar_mdfe_nao_encerrados.UseVisualStyleBackColor = false;
            this.bt_consultar_mdfe_nao_encerrados.Click += new System.EventHandler(this.bt_consultar_mdfe_nao_encerrados_Click);
            // 
            // bt_substituir_nfse
            // 
            this.bt_substituir_nfse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_substituir_nfse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_substituir_nfse.Location = new System.Drawing.Point(636, 306);
            this.bt_substituir_nfse.Name = "bt_substituir_nfse";
            this.bt_substituir_nfse.Size = new System.Drawing.Size(180, 23);
            this.bt_substituir_nfse.TabIndex = 38;
            this.bt_substituir_nfse.Text = "Substituir NFSe";
            this.bt_substituir_nfse.UseVisualStyleBackColor = false;
            this.bt_substituir_nfse.Click += new System.EventHandler(this.bt_substituir_nfse_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(837, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "CTe e CTeOS";
            // 
            // bt_consulta_status_cte
            // 
            this.bt_consulta_status_cte.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_consulta_status_cte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_consulta_status_cte.Location = new System.Drawing.Point(840, 25);
            this.bt_consulta_status_cte.Name = "bt_consulta_status_cte";
            this.bt_consulta_status_cte.Size = new System.Drawing.Size(180, 23);
            this.bt_consulta_status_cte.TabIndex = 40;
            this.bt_consulta_status_cte.Text = "Consulta Status";
            this.bt_consulta_status_cte.UseVisualStyleBackColor = false;
            this.bt_consulta_status_cte.Click += new System.EventHandler(this.bt_consulta_status_cte_Click);
            // 
            // bt_enviar_cte_assinc_lote
            // 
            this.bt_enviar_cte_assinc_lote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_enviar_cte_assinc_lote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_enviar_cte_assinc_lote.Location = new System.Drawing.Point(839, 54);
            this.bt_enviar_cte_assinc_lote.Name = "bt_enviar_cte_assinc_lote";
            this.bt_enviar_cte_assinc_lote.Size = new System.Drawing.Size(180, 23);
            this.bt_enviar_cte_assinc_lote.TabIndex = 41;
            this.bt_enviar_cte_assinc_lote.Text = "Enviar CTe Assíncrono em Lote";
            this.bt_enviar_cte_assinc_lote.UseVisualStyleBackColor = false;
            this.bt_enviar_cte_assinc_lote.Click += new System.EventHandler(this.bt_enviar_cte_assinc_lote_Click);
            // 
            // bt_enviar_cteos_sinc
            // 
            this.bt_enviar_cteos_sinc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_enviar_cteos_sinc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_enviar_cteos_sinc.Location = new System.Drawing.Point(839, 83);
            this.bt_enviar_cteos_sinc.Name = "bt_enviar_cteos_sinc";
            this.bt_enviar_cteos_sinc.Size = new System.Drawing.Size(180, 23);
            this.bt_enviar_cteos_sinc.TabIndex = 42;
            this.bt_enviar_cteos_sinc.Text = "Enviar CTeOS Síncrono";
            this.bt_enviar_cteos_sinc.UseVisualStyleBackColor = false;
            this.bt_enviar_cteos_sinc.Click += new System.EventHandler(this.bt_enviar_cteos_sinc_Click);
            // 
            // bt_enviar_evento_epec
            // 
            this.bt_enviar_evento_epec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_enviar_evento_epec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_enviar_evento_epec.Location = new System.Drawing.Point(12, 424);
            this.bt_enviar_evento_epec.Name = "bt_enviar_evento_epec";
            this.bt_enviar_evento_epec.Size = new System.Drawing.Size(180, 23);
            this.bt_enviar_evento_epec.TabIndex = 43;
            this.bt_enviar_evento_epec.Text = "Enviar Evento EPEC";
            this.bt_enviar_evento_epec.UseVisualStyleBackColor = false;
            this.bt_enviar_evento_epec.Click += new System.EventHandler(this.bt_enviar_evento_epec_Click);
            // 
            // bt_recuperar_xml_nfe_distribuicao
            // 
            this.bt_recuperar_xml_nfe_distribuicao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_recuperar_xml_nfe_distribuicao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_recuperar_xml_nfe_distribuicao.Location = new System.Drawing.Point(12, 527);
            this.bt_recuperar_xml_nfe_distribuicao.Name = "bt_recuperar_xml_nfe_distribuicao";
            this.bt_recuperar_xml_nfe_distribuicao.Size = new System.Drawing.Size(180, 23);
            this.bt_recuperar_xml_nfe_distribuicao.TabIndex = 44;
            this.bt_recuperar_xml_nfe_distribuicao.Text = "Recuperar XML Distribuição";
            this.bt_recuperar_xml_nfe_distribuicao.UseVisualStyleBackColor = false;
            this.bt_recuperar_xml_nfe_distribuicao.Click += new System.EventHandler(this.bt_recuperar_xml_nfe_distribuicao_Click);
            // 
            // bt_recuperar_xml_nfe_distribuicao_2
            // 
            this.bt_recuperar_xml_nfe_distribuicao_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bt_recuperar_xml_nfe_distribuicao_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_recuperar_xml_nfe_distribuicao_2.Location = new System.Drawing.Point(12, 556);
            this.bt_recuperar_xml_nfe_distribuicao_2.Name = "bt_recuperar_xml_nfe_distribuicao_2";
            this.bt_recuperar_xml_nfe_distribuicao_2.Size = new System.Drawing.Size(180, 23);
            this.bt_recuperar_xml_nfe_distribuicao_2.TabIndex = 45;
            this.bt_recuperar_xml_nfe_distribuicao_2.Text = "Recuperar XML Distribuição 2";
            this.bt_recuperar_xml_nfe_distribuicao_2.UseVisualStyleBackColor = false;
            this.bt_recuperar_xml_nfe_distribuicao_2.Click += new System.EventHandler(this.bt_recuperar_xml_nfe_distribuicao_2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1032, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "Diversos";
            // 
            // bt_imprimir_danfe_etiqueta
            // 
            this.bt_imprimir_danfe_etiqueta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bt_imprimir_danfe_etiqueta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_imprimir_danfe_etiqueta.Location = new System.Drawing.Point(1035, 54);
            this.bt_imprimir_danfe_etiqueta.Name = "bt_imprimir_danfe_etiqueta";
            this.bt_imprimir_danfe_etiqueta.Size = new System.Drawing.Size(180, 23);
            this.bt_imprimir_danfe_etiqueta.TabIndex = 47;
            this.bt_imprimir_danfe_etiqueta.Text = "Imprimir DANFe Etiqueta";
            this.bt_imprimir_danfe_etiqueta.UseVisualStyleBackColor = false;
            this.bt_imprimir_danfe_etiqueta.Click += new System.EventHandler(this.bt_imprimir_danfe_etiqueta_Click);
            // 
            // bt_carregar_cert_a3
            // 
            this.bt_carregar_cert_a3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bt_carregar_cert_a3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_carregar_cert_a3.Location = new System.Drawing.Point(1035, 25);
            this.bt_carregar_cert_a3.Name = "bt_carregar_cert_a3";
            this.bt_carregar_cert_a3.Size = new System.Drawing.Size(180, 23);
            this.bt_carregar_cert_a3.TabIndex = 48;
            this.bt_carregar_cert_a3.Text = "Carregar Certificado A3 com PIN";
            this.bt_carregar_cert_a3.UseVisualStyleBackColor = false;
            this.bt_carregar_cert_a3.Click += new System.EventHandler(this.bt_carregar_cert_a3_Click);
            // 
            // bt_evento_canc_cte
            // 
            this.bt_evento_canc_cte.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_evento_canc_cte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_evento_canc_cte.Location = new System.Drawing.Point(839, 112);
            this.bt_evento_canc_cte.Name = "bt_evento_canc_cte";
            this.bt_evento_canc_cte.Size = new System.Drawing.Size(180, 23);
            this.bt_evento_canc_cte.TabIndex = 49;
            this.bt_evento_canc_cte.Text = "Evento de Cancelamento CTe";
            this.bt_evento_canc_cte.UseVisualStyleBackColor = false;
            this.bt_evento_canc_cte.Click += new System.EventHandler(this.bt_evento_canc_cte_Click);
            // 
            // bt_evento_canc_cteos
            // 
            this.bt_evento_canc_cteos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_evento_canc_cteos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_evento_canc_cteos.Location = new System.Drawing.Point(839, 141);
            this.bt_evento_canc_cteos.Name = "bt_evento_canc_cteos";
            this.bt_evento_canc_cteos.Size = new System.Drawing.Size(180, 23);
            this.bt_evento_canc_cteos.TabIndex = 50;
            this.bt_evento_canc_cteos.Text = "Evento de Cancelamento CTeOS";
            this.bt_evento_canc_cteos.UseVisualStyleBackColor = false;
            this.bt_evento_canc_cteos.Click += new System.EventHandler(this.bt_evento_canc_cteos_Click);
            // 
            // bt_evento_cce_e_cte
            // 
            this.bt_evento_cce_e_cte.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_evento_cce_e_cte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_evento_cce_e_cte.Location = new System.Drawing.Point(839, 170);
            this.bt_evento_cce_e_cte.Name = "bt_evento_cce_e_cte";
            this.bt_evento_cce_e_cte.Size = new System.Drawing.Size(180, 23);
            this.bt_evento_cce_e_cte.TabIndex = 51;
            this.bt_evento_cce_e_cte.Text = "Evento de CCe e CTe";
            this.bt_evento_cce_e_cte.UseVisualStyleBackColor = false;
            this.bt_evento_cce_e_cte.Click += new System.EventHandler(this.bt_evento_cce_e_cte_Click);
            // 
            // bt_evento_cce_e_cteos
            // 
            this.bt_evento_cce_e_cteos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bt_evento_cce_e_cteos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_evento_cce_e_cteos.Location = new System.Drawing.Point(839, 199);
            this.bt_evento_cce_e_cteos.Name = "bt_evento_cce_e_cteos";
            this.bt_evento_cce_e_cteos.Size = new System.Drawing.Size(180, 23);
            this.bt_evento_cce_e_cteos.TabIndex = 52;
            this.bt_evento_cce_e_cteos.Text = "Evento de CCe e CTeOS";
            this.bt_evento_cce_e_cteos.UseVisualStyleBackColor = false;
            this.bt_evento_cce_e_cteos.Click += new System.EventHandler(this.bt_evento_cce_e_cteos_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1035, 83);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 38);
            this.button1.TabIndex = 53;
            this.button1.Text = "Formas de Trabalhar com Certificado Digital";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1229, 605);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_evento_cce_e_cteos);
            this.Controls.Add(this.bt_evento_cce_e_cte);
            this.Controls.Add(this.bt_evento_canc_cteos);
            this.Controls.Add(this.bt_evento_canc_cte);
            this.Controls.Add(this.bt_carregar_cert_a3);
            this.Controls.Add(this.bt_imprimir_danfe_etiqueta);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.bt_recuperar_xml_nfe_distribuicao_2);
            this.Controls.Add(this.bt_recuperar_xml_nfe_distribuicao);
            this.Controls.Add(this.bt_enviar_evento_epec);
            this.Controls.Add(this.bt_enviar_cteos_sinc);
            this.Controls.Add(this.bt_enviar_cte_assinc_lote);
            this.Controls.Add(this.bt_consulta_status_cte);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bt_substituir_nfse);
            this.Controls.Add(this.bt_consultar_mdfe_nao_encerrados);
            this.Controls.Add(this.bt_enviar_evento_encerramento);
            this.Controls.Add(this.bt_consultar_lote_rps);
            this.Controls.Add(this.bt_consultar_nfse_por_rps);
            this.Controls.Add(this.bt_enviar_evento_cancelamento);
            this.Controls.Add(this.bt_resgatar_xml_da_nfce);
            this.Controls.Add(this.bt_gerar_nfce_contingencia);
            this.Controls.Add(this.bt_env_evento_canc_substituicao);
            this.Controls.Add(this.bt_env_lote_rps_assinc);
            this.Controls.Add(this.bt_env_lote_rps_sinc);
            this.Controls.Add(this.bt_env_rps_sinc);
            this.Controls.Add(this.bt_cancelar_nfse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bt_env_nfce_contingencia_offline);
            this.Controls.Add(this.bt_exe_config_danfe);
            this.Controls.Add(this.bt_imprimir_danfe);
            this.Controls.Add(this.bt_env_mdfe_sinc);
            this.Controls.Add(this.bt_env_mdfe_assinc);
            this.Controls.Add(this.bt_consulta_situacao_mdfe);
            this.Controls.Add(this.bt_consulta_status_mdfe);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bt_env_nfce_sinc);
            this.Controls.Add(this.bt_consulta_situacao_nfce);
            this.Controls.Add(this.bt_consulta_status_nfce);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt_env_evento_cce);
            this.Controls.Add(this.bt_env_evento_cancel_nfe);
            this.Controls.Add(this.bt_env_nfe_desserializacao);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bt_cons_cadas_contribuinte);
            this.Controls.Add(this.bt_inutilizacao);
            this.Controls.Add(this.bt_env_nfe_assinc_lote);
            this.Controls.Add(this.bt_env_nfe_assinc);
            this.Controls.Add(this.bt_env_nfe_sinc);
            this.Controls.Add(this.bt_consulta_situacao_NFe);
            this.Controls.Add(this.bt_consulta_status);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serviços";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_consulta_status;
        private System.Windows.Forms.Button bt_consulta_situacao_NFe;
        private System.Windows.Forms.Button bt_env_nfe_sinc;
        private System.Windows.Forms.Button bt_env_nfe_assinc;
        private System.Windows.Forms.Button bt_env_nfe_assinc_lote;
        private System.Windows.Forms.Button bt_inutilizacao;
        private System.Windows.Forms.Button bt_cons_cadas_contribuinte;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_env_nfe_desserializacao;
        private System.Windows.Forms.Button bt_env_evento_cancel_nfe;
        private System.Windows.Forms.Button bt_env_evento_cce;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_consulta_status_nfce;
        private System.Windows.Forms.Button bt_consulta_situacao_nfce;
        private System.Windows.Forms.Button bt_env_nfce_sinc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bt_env_mdfe_assinc;
        private System.Windows.Forms.Button bt_consulta_situacao_mdfe;
        private System.Windows.Forms.Button bt_consulta_status_mdfe;
        private System.Windows.Forms.Button bt_env_mdfe_sinc;
        private System.Windows.Forms.Button bt_imprimir_danfe;
        private System.Windows.Forms.Button bt_exe_config_danfe;
        private System.Windows.Forms.Button bt_env_nfce_contingencia_offline;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bt_env_lote_rps_assinc;
        private System.Windows.Forms.Button bt_env_lote_rps_sinc;
        private System.Windows.Forms.Button bt_env_rps_sinc;
        private System.Windows.Forms.Button bt_cancelar_nfse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bt_env_evento_canc_substituicao;
        private System.Windows.Forms.Button bt_gerar_nfce_contingencia;
        private System.Windows.Forms.Button bt_resgatar_xml_da_nfce;
        private System.Windows.Forms.Button bt_enviar_evento_cancelamento;
        private System.Windows.Forms.Button bt_consultar_nfse_por_rps;
        private System.Windows.Forms.Button bt_consultar_lote_rps;
        private System.Windows.Forms.Button bt_enviar_evento_encerramento;
        private System.Windows.Forms.Button bt_consultar_mdfe_nao_encerrados;
        private System.Windows.Forms.Button bt_substituir_nfse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bt_consulta_status_cte;
        private System.Windows.Forms.Button bt_enviar_cte_assinc_lote;
        private System.Windows.Forms.Button bt_enviar_cteos_sinc;
        private System.Windows.Forms.Button bt_enviar_evento_epec;
        private System.Windows.Forms.Button bt_recuperar_xml_nfe_distribuicao;
        private System.Windows.Forms.Button bt_recuperar_xml_nfe_distribuicao_2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt_imprimir_danfe_etiqueta;
        private System.Windows.Forms.Button bt_carregar_cert_a3;
        private System.Windows.Forms.Button bt_evento_canc_cte;
        private System.Windows.Forms.Button bt_evento_canc_cteos;
        private System.Windows.Forms.Button bt_evento_cce_e_cte;
        private System.Windows.Forms.Button bt_evento_cce_e_cteos;
        private System.Windows.Forms.Button button1;
    }
}

