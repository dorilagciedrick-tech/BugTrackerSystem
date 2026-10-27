using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTrackerApp
{
    public class MainForm : Form
    {
        private readonly BugManager manager = new();
        private readonly TextBox reporterTextBox = new();
        private readonly TextBox courseTextBox = new();
        private readonly ComboBox featureComboBox = new();
        private readonly ComboBox severityComboBox = new();
        private readonly ComboBox environmentComboBox = new();
        private readonly TextBox bugIdTextBox = new();
        private readonly TextBox summaryTextBox = new();
        private readonly ListBox displayListBox = new();
        private readonly ComboBox viewComboBox = new();
        private readonly Button submitButton = new();
        private readonly Button sendToReviewButton = new();
        private readonly Button returnToBacklogButton = new();
        private readonly Button sprintCloseButton = new();
        private readonly Button benchmarkButton = new();
        private readonly TextBox benchmarkOutputTextBox = new();
        private readonly RichTextBox aiChatHistory = new();
        private readonly TextBox aiInput = new();
        private readonly Button aiSendButton = new();

        public MainForm()
        {
            Text = "Quipper Bug Tracker";
            Width = 1380;
            Height = 860;
            MinimumSize = new Size(1280, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(238, 244, 252);
            Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            InitializeControls();
            RefreshDisplay();
        }

        private void InitializeControls()
        {
            var banner = new Label
            {
                Text = "Quipper Support Hub",
                AutoSize = false,
                Height = 46,
                Width = 450,
                Left = 20,
                Top = 12,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(18, 69, 120),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var reporterLabel = new Label { Text = "Reporter / Student Name:", Left = 20, Top = 72, Width = 210, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            reporterTextBox.SetBounds(240, 68, 360, 32);
            reporterTextBox.BorderStyle = BorderStyle.FixedSingle;
            reporterTextBox.BackColor = Color.White;
            reporterTextBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            var courseLabel = new Label { Text = "Course / Class:", Left = 20, Top = 118, Width = 210, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            courseTextBox.SetBounds(240, 114, 360, 32);
            courseTextBox.BorderStyle = BorderStyle.FixedSingle;
            courseTextBox.BackColor = Color.White;
            courseTextBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            var featureLabel = new Label { Text = "Quipper Feature:", Left = 20, Top = 164, Width = 210, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            featureComboBox.SetBounds(240, 160, 260, 32);
            featureComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            featureComboBox.Items.AddRange(new object[] { "Assignments", "Assessments", "Dashboard", "Videos", "Quizzes", "Reports", "Notifications", "Profile", "Payments", "Other" });
            featureComboBox.SelectedIndex = 0;
            featureComboBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            featureComboBox.BackColor = Color.White;

            var severityLabel = new Label { Text = "Priority:", Left = 620, Top = 72, Width = 120, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            severityComboBox.SetBounds(740, 68, 180, 32);
            severityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            severityComboBox.Items.AddRange(new object[] { "Low", "Medium", "High", "Critical" });
            severityComboBox.SelectedIndex = 1;
            severityComboBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            severityComboBox.BackColor = Color.White;

            var environmentLabel = new Label { Text = "Platform:", Left = 620, Top = 118, Width = 120, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            environmentComboBox.SetBounds(740, 114, 180, 32);
            environmentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            environmentComboBox.Items.AddRange(new object[] { "Web", "Mobile App", "Tablet", "Desktop", "Unknown" });
            environmentComboBox.SelectedIndex = 0;
            environmentComboBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            environmentComboBox.BackColor = Color.White;

            var bugIdLabel = new Label { Text = "Issue ID / Time:", Left = 560, Top = 164, Width = 170, Height = 28, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            bugIdTextBox.SetBounds(740, 160, 180, 32);
            bugIdTextBox.BorderStyle = BorderStyle.FixedSingle;
            bugIdTextBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            var summaryLabel = new Label { Text = "What went wrong?", Left = 20, Top = 220, Width = 260, Height = 32, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94), TextAlign = ContentAlignment.MiddleLeft };
            summaryTextBox.SetBounds(20, 252, 860, 110);
            summaryTextBox.Multiline = true;
            summaryTextBox.ScrollBars = ScrollBars.Vertical;
            summaryTextBox.BorderStyle = BorderStyle.FixedSingle;
            summaryTextBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            summaryTextBox.BackColor = Color.White;

            var buttonColor = Color.FromArgb(52, 124, 235);
            var buttonHoverColor = Color.FromArgb(31, 100, 218);

            ConfigureButton(submitButton, "Submit Bug", 20, 382, 180, 40, buttonColor, buttonHoverColor);
            submitButton.Click += SubmitButton_Click;

            ConfigureButton(sendToReviewButton, "Send to Review", 220, 382, 180, 40, buttonColor, buttonHoverColor);
            sendToReviewButton.Click += SendToReviewButton_Click;

            ConfigureButton(returnToBacklogButton, "Return to Backlog", 420, 382, 190, 40, buttonColor, buttonHoverColor);
            returnToBacklogButton.Click += ReturnToBacklogButton_Click;

            ConfigureButton(sprintCloseButton, "Sprint Close", 640, 382, 170, 40, Color.FromArgb(0, 123, 255), Color.FromArgb(0, 96, 210));
            sprintCloseButton.Click += SprintCloseButton_Click;

            ConfigureButton(benchmarkButton, "Benchmark", 830, 382, 120, 40, Color.FromArgb(0, 153, 76), Color.FromArgb(0, 128, 64));
            benchmarkButton.Click += BenchmarkButton_Click;

            viewComboBox.SetBounds(150, 438, 260, 32);
            viewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            viewComboBox.Items.AddRange(new[] { "Backlog", "Review Board" });
            viewComboBox.SelectedIndex = 0;
            viewComboBox.SelectedIndexChanged += ViewComboBox_SelectedIndexChanged;
            viewComboBox.BackColor = Color.White;
            viewComboBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var viewLabel = new Label { Text = "View List:", Left = 28, Top = 442, Width = 110, Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(17, 48, 94) };

            displayListBox.SetBounds(28, 490, 920, 180);
            displayListBox.HorizontalScrollbar = true;
            displayListBox.SelectionMode = SelectionMode.One;
            displayListBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            displayListBox.BackColor = Color.White;
            displayListBox.DoubleClick += DisplayListBox_DoubleClick;
            displayListBox.BorderStyle = BorderStyle.FixedSingle;

            benchmarkOutputTextBox.SetBounds(28, 690, 920, 120);
            benchmarkOutputTextBox.Multiline = true;
            benchmarkOutputTextBox.ScrollBars = ScrollBars.Vertical;
            benchmarkOutputTextBox.ReadOnly = true;
            benchmarkOutputTextBox.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            benchmarkOutputTextBox.BackColor = Color.White;
            benchmarkOutputTextBox.BorderStyle = BorderStyle.FixedSingle;
            benchmarkOutputTextBox.Text = "Benchmark results will appear here.";

            aiChatHistory.SetBounds(968, 24, 300, 620);
            aiChatHistory.Multiline = true;
            aiChatHistory.ReadOnly = true;
            aiChatHistory.ScrollBars = RichTextBoxScrollBars.Vertical;
            aiChatHistory.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
            aiChatHistory.BackColor = Color.White;
            aiChatHistory.Text = "Quipper AI: I’m ready to help diagnose the issue.\r\n";

            aiInput.SetBounds(968, 660, 220, 30);
            aiInput.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            aiInput.Name = "aiInput";

            ConfigureButton(aiSendButton, "Ask AI", 1198, 660, 70, 30, Color.FromArgb(52, 124, 235), Color.FromArgb(31, 100, 218));
            aiSendButton.Name = "aiSendButton";
            aiSendButton.Click += AskButton_Click;

            aiChatHistory.Name = "aiChatHistory";

            Controls.AddRange(new Control[]
            {
                banner,
                reporterLabel,
                reporterTextBox,
                courseLabel,
                courseTextBox,
                featureLabel,
                featureComboBox,
                severityLabel,
                severityComboBox,
                environmentLabel,
                environmentComboBox,
                bugIdLabel,
                bugIdTextBox,
                summaryLabel,
                summaryTextBox,
                submitButton,
                sendToReviewButton,
                returnToBacklogButton,
                sprintCloseButton,
                benchmarkButton,
                viewLabel,
                viewComboBox,
                displayListBox,
                benchmarkOutputTextBox,
                aiChatHistory,
                aiInput,
                aiSendButton
            });

            Resize += (_, _) => UpdateLayoutForSize();
            UpdateLayoutForSize();
        }

        private void UpdateLayoutForSize()
        {
            int rightPanelWidth = 300;
            int rightPanelLeft = ClientSize.Width - rightPanelWidth - 22;
            int rightPanelTop = 24;

            aiChatHistory.SetBounds(rightPanelLeft, rightPanelTop, rightPanelWidth, Math.Max(440, ClientSize.Height - 190));
            aiInput.SetBounds(rightPanelLeft, aiChatHistory.Bottom + 12, rightPanelWidth - 90, 30);
            aiSendButton.SetBounds(rightPanelLeft + rightPanelWidth - 86, aiInput.Top, 76, 30);

            int leftWidth = Math.Max(680, aiChatHistory.Left - 40);
            summaryTextBox.Width = leftWidth;
            displayListBox.Width = leftWidth;
            benchmarkOutputTextBox.Width = leftWidth;
        }

        private static void ConfigureButton(Button button, string text, int left, int top, int width, int height, Color backColor, Color hoverColor)
        {
            button.Text = text;
            button.SetBounds(left, top, width, height);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            button.BackColor = backColor;
            button.Cursor = Cursors.Hand;
            button.MouseEnter += (_, _) => button.BackColor = hoverColor;
            button.MouseLeave += (_, _) => button.BackColor = backColor;
        }

        private void SubmitButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(reporterTextBox.Text) || string.IsNullOrWhiteSpace(bugIdTextBox.Text) || string.IsNullOrWhiteSpace(summaryTextBox.Text))
            {
                MessageBox.Show("Please provide the reporter, issue ID, and a clear bug description before submitting.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bugId = bugIdTextBox.Text.Trim();
            var existsInBacklog = manager.Backlog.Any(b => string.Equals(b.BugID?.Trim(), bugId, StringComparison.OrdinalIgnoreCase));
            var existsInReview = manager.ReviewBoard.Any(b => string.Equals(b.BugID?.Trim(), bugId, StringComparison.OrdinalIgnoreCase));

            if (existsInBacklog || existsInReview)
            {
                MessageBox.Show($"A bug with ID '{bugId}' already exists in Quipper support. Please use a unique issue ID.", "Duplicate Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var report = new BugReport(
                reporterTextBox.Text.Trim(),
                bugId,
                summaryTextBox.Text.Trim(),
                courseTextBox.Text.Trim(),
                featureComboBox.SelectedItem?.ToString(),
                severityComboBox.SelectedItem?.ToString(),
                environmentComboBox.SelectedItem?.ToString());

            manager.SubmitBug(report);
            ClearInputs();
            RefreshDisplay();
        }

        private void SendToReviewButton_Click(object? sender, EventArgs e)
        {
            var selectedReport = GetSelectedReport();
            if (selectedReport == null)
            {
                MessageBox.Show("Select a backlog item to send to review.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (manager.SendToReview(selectedReport))
            {
                RefreshDisplay();
            }
        }

        private void ReturnToBacklogButton_Click(object? sender, EventArgs e)
        {
            var selectedReport = GetSelectedReport();
            if (selectedReport == null)
            {
                MessageBox.Show("Select a review item to return to backlog.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (manager.ReturnToBacklog(selectedReport))
            {
                RefreshDisplay();
            }
        }

        private void SprintCloseButton_Click(object? sender, EventArgs e)
        {
            manager.SprintClose();
            RefreshDisplay();

            var summary = "Sprint close completed. The Quipper backlog now contains:\r\n" + string.Join("\r\n", manager.Backlog.Select(item => item.ToString()));
            MessageBox.Show(summary, "Sprint Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BenchmarkButton_Click(object? sender, EventArgs e)
        {
            benchmarkOutputTextBox.Text = CollectionBenchmark.BuildReport();
            benchmarkOutputTextBox.SelectionStart = benchmarkOutputTextBox.TextLength;
            benchmarkOutputTextBox.ScrollToCaret();
        }

        private void ViewComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private BugReport? GetSelectedReport()
        {
            return displayListBox.SelectedItem as BugReport;
        }

        private void RefreshDisplay()
        {
            displayListBox.Items.Clear();
            var list = viewComboBox.SelectedItem?.ToString();

            if (list == "Review Board")
            {
                displayListBox.Items.AddRange(manager.ReviewBoard.Cast<object>().ToArray());
            }
            else
            {
                displayListBox.Items.AddRange(manager.Backlog.Cast<object>().ToArray());
            }
        }

        private void ClearInputs()
        {
            reporterTextBox.Clear();
            courseTextBox.Clear();
            bugIdTextBox.Clear();
            summaryTextBox.Clear();
            featureComboBox.SelectedIndex = 0;
            severityComboBox.SelectedIndex = 1;
            environmentComboBox.SelectedIndex = 0;
            reporterTextBox.Focus();
        }

        private static string BuildQuipperBugContext(BugReport report)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Quipper bug context:");
            builder.AppendLine($"Reporter: {report.ReporterName}");
            builder.AppendLine($"Course/Class: {report.CourseName}");
            builder.AppendLine($"Feature: {report.FeatureArea}");
            builder.AppendLine($"Severity: {report.Severity}");
            builder.AppendLine($"Platform: {report.Environment}");
            builder.AppendLine($"Issue ID / Time: {report.BugID}");
            builder.AppendLine($"Problem description: {report.Summary}");
            return builder.ToString();
        }

        private static string BuildQuipperPrompt(string userQuery, BugReport? selectedBug)
        {
            var builder = new StringBuilder();
            builder.AppendLine("You are the Quipper support triage assistant for a learning platform used by students, teachers, and administrators.");
            builder.AppendLine("Focus on likely technical causes in Quipper features such as assignments, assessments, quizzes, video lessons, reports, notifications, login, and dashboard flows.");
            builder.AppendLine("Suggest the most likely root cause and the first 3 to 6 actions a support engineer or developer should take.");
            builder.AppendLine("Keep the answer concise, prioritized, and specific to the exact issue described.");

            if (selectedBug != null)
            {
                builder.AppendLine();
                builder.AppendLine(BuildQuipperBugContext(selectedBug));
            }

            builder.AppendLine();
            builder.AppendLine($"User request: {userQuery}");
            return builder.ToString();
        }

        private void AppendToAiLog(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            try
            {
                if (aiChatHistory.InvokeRequired)
                {
                    aiChatHistory.Invoke((Action)(() =>
                    {
                        aiChatHistory.AppendText(text);
                        aiChatHistory.SelectionStart = aiChatHistory.TextLength;
                        aiChatHistory.ScrollToCaret();
                    }));
                }
                else
                {
                    aiChatHistory.AppendText(text);
                    aiChatHistory.SelectionStart = aiChatHistory.TextLength;
                    aiChatHistory.ScrollToCaret();
                }
            }
            catch
            {
            }
        }

        private async void DisplayListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (viewComboBox.SelectedItem?.ToString() != "Review Board")
            {
                AppendToAiLog("[AI] Double-click only works for bugs in the Review Board.\r\n");
                return;
            }

            var selected = GetSelectedReport();
            if (selected == null)
            {
                AppendToAiLog("[AI] Please select a bug first.\r\n");
                return;
            }

            try { aiSendButton.Invoke((Action)(() => aiSendButton.Enabled = false)); } catch { aiSendButton.Enabled = false; }

            AppendToAiLog($"[User] {selected}\r\n");

            var progress = new Progress<string>(chunk =>
            {
                if (string.IsNullOrEmpty(chunk)) return;
                AppendToAiLog(chunk);
            });

            try
            {
                using var client = new OllamaClient();
                var prompt = BuildQuipperPrompt("Analyze this Quipper bug and suggest the first troubleshooting steps.", selected);
                await client.SendChatAsync(prompt, "llama3.2", null, CancellationToken.None, progress).ConfigureAwait(false);
                AppendToAiLog("\r\n[AI Completed]\r\n");
            }
            catch (Exception ex)
            {
                AppendToAiLog($"\r\n[AI Error] {ex.Message}\r\n");
            }
            finally
            {
                try { aiSendButton.Invoke((Action)(() => aiSendButton.Enabled = true)); } catch { aiSendButton.Enabled = true; }
            }
        }

        private async void AskButton_Click(object? sender, EventArgs e)
        {
            var inputCtrl = this.Controls.Find("aiInput", true).FirstOrDefault() as TextBox;
            var sendBtn = (sender as Button) ?? this.Controls.Find("aiSendButton", true).FirstOrDefault() as Button;
            var logCtrl = this.Controls.Find("aiChatHistory", true).FirstOrDefault();

            if (inputCtrl == null || sendBtn == null || logCtrl == null)
            {
                return;
            }

            var query = inputCtrl.Text?.Trim();
            if (string.IsNullOrEmpty(query)) return;

            sendBtn.Enabled = false;
            this.Invoke((Action)(() => inputCtrl.Clear()));

            void AppendToLog(string text)
            {
                this.Invoke((Action)(() =>
                {
                    if (logCtrl is RichTextBox rtb)
                    {
                        rtb.AppendText(text);
                        rtb.SelectionStart = rtb.TextLength;
                        rtb.ScrollToCaret();
                    }
                    else if (logCtrl is TextBox tb)
                    {
                        tb.AppendText(text);
                        tb.SelectionStart = tb.TextLength;
                        tb.ScrollToCaret();
                    }
                }));
            }

            AppendToLog($"[User] {query}\r\n");

            var selectedBug = GetSelectedReport();
            var progress = new Progress<string>(chunk =>
            {
                if (string.IsNullOrEmpty(chunk)) return;
                AppendToLog(chunk);
            });

            try
            {
                using var client = new OllamaClient();
                var prompt = BuildQuipperPrompt(query, selectedBug);
                await client.SendChatAsync(prompt, "llama3.2", null, CancellationToken.None, progress).ConfigureAwait(false);
                AppendToLog("\r\n[AI Completed]\r\n");
            }
            catch (Exception ex)
            {
                AppendToLog($"\r\n[AI Error] {ex.Message}\r\n");
            }
            finally
            {
                this.Invoke((Action)(() => sendBtn.Enabled = true));
            }
        }
    }
}
