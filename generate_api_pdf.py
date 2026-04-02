from fpdf import FPDF
import os

OUTPUT = r"c:\Users\yashs\Documents\ok\DigitalWallet_API_Documentation.pdf"

class PDF(FPDF):
    def header(self):
        self.set_font("Helvetica", "B", 10)
        self.set_text_color(80, 80, 80)
        self.cell(0, 8, "DigitalWallet API Documentation", align="R")
        self.ln(4)
        self.set_draw_color(200, 200, 200)
        self.line(10, self.get_y(), 200, self.get_y())
        self.ln(4)

    def footer(self):
        self.set_y(-13)
        self.set_font("Helvetica", "I", 8)
        self.set_text_color(150, 150, 150)
        self.cell(0, 10, f"Page {self.page_no()}", align="C")

    def section_title(self, title):
        self.ln(4)
        self.set_fill_color(30, 60, 114)
        self.set_text_color(255, 255, 255)
        self.set_font("Helvetica", "B", 12)
        self.cell(0, 9, f"  {title}", fill=True, ln=True)
        self.set_text_color(0, 0, 0)
        self.ln(2)

    def sub_title(self, title):
        self.ln(3)
        self.set_fill_color(220, 235, 255)
        self.set_text_color(20, 40, 90)
        self.set_font("Helvetica", "B", 10)
        self.cell(0, 7, f"  {title}", fill=True, ln=True)
        self.set_text_color(0, 0, 0)
        self.ln(1)

    def endpoint_row(self, method, path, desc):
        colors = {
            "GET":    (40, 167, 69),
            "POST":   (0, 123, 255),
            "PUT":    (255, 153, 0),
            "PATCH":  (111, 66, 193),
            "DELETE": (220, 53, 69),
        }
        bg = colors.get(method, (100, 100, 100))
        # Method badge
        self.set_fill_color(*bg)
        self.set_text_color(255, 255, 255)
        self.set_font("Helvetica", "B", 8)
        self.cell(18, 6, method, fill=True, align="C")
        # Path
        self.set_fill_color(245, 245, 245)
        self.set_text_color(30, 30, 30)
        self.set_font("Courier", "", 8)
        self.cell(90, 6, f"  {path}", fill=True)
        # Description
        self.set_font("Helvetica", "", 8)
        self.set_fill_color(255, 255, 255)
        self.multi_cell(0, 6, desc, fill=True)
        self.ln(1)

    def note(self, text):
        self.set_font("Helvetica", "I", 8)
        self.set_text_color(100, 100, 100)
        self.multi_cell(0, 5, text)
        self.set_text_color(0, 0, 0)

    def code_block(self, label, code):
        if label:
            self.set_font("Helvetica", "B", 8)
            self.set_text_color(50, 50, 50)
            self.cell(0, 5, label, ln=True)
        self.set_fill_color(30, 30, 30)
        self.set_text_color(180, 220, 150)
        self.set_font("Courier", "", 7)
        self.multi_cell(0, 4.5, code.strip(), fill=True)
        self.set_text_color(0, 0, 0)
        self.ln(2)

    def label(self, text):
        self.set_font("Helvetica", "B", 8)
        self.set_text_color(60, 60, 60)
        self.cell(0, 5, text, ln=True)

    def body(self, text):
        self.set_font("Helvetica", "", 9)
        self.set_text_color(30, 30, 30)
        self.multi_cell(0, 5, text)
        self.ln(1)

    def rate_table(self, rows):
        self.set_font("Helvetica", "B", 8)
        self.set_fill_color(30, 60, 114)
        self.set_text_color(255, 255, 255)
        self.cell(80, 6, "  Service", fill=True)
        self.cell(40, 6, "Limit / min", fill=True)
        self.cell(0, 6, "Auth Required", fill=True, ln=True)
        for i, (svc, limit, auth) in enumerate(rows):
            bg = (245, 248, 255) if i % 2 == 0 else (255, 255, 255)
            self.set_fill_color(*bg)
            self.set_text_color(30, 30, 30)
            self.set_font("Helvetica", "", 8)
            self.cell(80, 6, f"  {svc}", fill=True)
            self.cell(40, 6, limit, fill=True)
            self.cell(0, 6, auth, fill=True, ln=True)
        self.ln(3)

    def port_table(self, rows):
        self.set_font("Helvetica", "B", 8)
        self.set_fill_color(30, 60, 114)
        self.set_text_color(255, 255, 255)
        self.cell(80, 6, "  Service", fill=True)
        self.cell(0, 6, "Port", fill=True, ln=True)
        for i, (svc, port) in enumerate(rows):
            bg = (245, 248, 255) if i % 2 == 0 else (255, 255, 255)
            self.set_fill_color(*bg)
            self.set_text_color(30, 30, 30)
            self.set_font("Helvetica", "", 8)
            self.cell(80, 6, f"  {svc}", fill=True)
            self.cell(0, 6, port, fill=True, ln=True)
        self.ln(3)


pdf = PDF()
pdf.set_auto_page_break(auto=True, margin=15)
pdf.add_page()

# ── Cover ──────────────────────────────────────────────────────────────────────
pdf.set_font("Helvetica", "B", 26)
pdf.set_text_color(30, 60, 114)
pdf.ln(20)
pdf.cell(0, 14, "DigitalWallet", align="C", ln=True)
pdf.set_font("Helvetica", "B", 18)
pdf.set_text_color(60, 100, 180)
pdf.cell(0, 10, "API Documentation", align="C", ln=True)
pdf.set_font("Helvetica", "", 10)
pdf.set_text_color(120, 120, 120)
pdf.cell(0, 7, "Microservices Architecture  |  REST API  |  v1.0", align="C", ln=True)
pdf.ln(6)
pdf.set_draw_color(30, 60, 114)
pdf.set_line_width(0.8)
pdf.line(30, pdf.get_y(), 180, pdf.get_y())
pdf.ln(10)

# Overview box
pdf.set_fill_color(240, 245, 255)
pdf.set_text_color(30, 30, 30)
pdf.set_font("Helvetica", "B", 9)
pdf.cell(0, 6, "  Overview", ln=True, fill=True)
pdf.set_font("Helvetica", "", 9)
pdf.set_fill_color(248, 250, 255)
lines = [
    "  Base URL (Gateway):  http://localhost:5000",
    "  All responses:       { \"success\": bool, \"data\": <payload>, \"message\": string }",
    "  Authentication:      JWT Bearer token -- Authorization: Bearer <accessToken>",
    "  Services:            Auth | Wallet | Rewards | Notifications | Admin | Support",
]
for l in lines:
    pdf.cell(0, 6, l, ln=True, fill=True)
pdf.ln(6)

pdf.set_text_color(0,0,0)

# ══════════════════════════════════════════════════════════════════════════════
# 1. AUTH SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("1. Auth Service  --  /api/auth")
pdf.body("Handles user registration, login, token management, OTP, and KYC submission.\nNo authentication required except for KYC endpoints (marked with lock).")

# signup
pdf.sub_title("POST /api/auth/signup  --  Register new user")
pdf.code_block("Request Body:", """{
  "email": "john@example.com",
  "phone": "+919876543210",
  "password": "Secret@123"
}""")
pdf.code_block("Response 201:", """{
  "success": true,
  "data": {
    "accessToken": "eyJhbGci...",
    "refreshToken": "dGhpcyBp...",
    "expiresAt": "2026-04-01T06:30:00Z",
    "user": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "john@example.com",
      "phone": "+919876543210",
      "role": "User",
      "isActive": true,
      "createdAt": "2026-03-31T10:00:00Z"
    }
  }
}""")

# login
pdf.sub_title("POST /api/auth/login  --  Login with credentials")
pdf.code_block("Request Body:", """{
  "email": "john@example.com",
  "password": "Secret@123"
}""")
pdf.note("Response 200: Same shape as /signup response.")
pdf.ln(2)

# refresh
pdf.sub_title("POST /api/auth/refresh-token  --  Refresh access token")
pdf.code_block("Request Body:", """{
  "refreshToken": "dGhpcyBp..."
}""")
pdf.note("Response 200: Same shape as /signup response with new tokens.")
pdf.ln(2)

# logout
pdf.sub_title("POST /api/auth/logout  --  Logout and revoke token")
pdf.code_block("Request Body:", """{
  "refreshToken": "dGhpcyBp..."
}""")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": "Logged out successfully"
}""")

# OTP
pdf.sub_title("POST /api/auth/otp/send  --  Send OTP to email")
pdf.code_block("Request Body:", '{ "email": "john@example.com" }')
pdf.code_block("Response 200:", '{ "success": true, "data": { "code": "483920" } }')
pdf.note("Note: 'code' field is only present in Development environment.")
pdf.ln(2)

pdf.sub_title("POST /api/auth/otp/verify  --  Verify OTP code")
pdf.code_block("Request Body:", '{ "email": "john@example.com", "code": "483920" }')
pdf.code_block("Response 200:", '{ "success": true, "data": "OTP verified successfully" }')

# KYC
pdf.sub_title("POST /api/auth/kyc/submit  [Auth Required]  --  Submit KYC document")
pdf.code_block("Request Body:", """{
  "docType": "Passport",
  "fileUrl": "https://storage.example.com/docs/passport_john.pdf"
}""")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "documentId": "a1b2c3d4-...",
    "docType": "Passport",
    "status": "Pending",
    "submittedAt": "2026-03-31T10:05:00Z"
  }
}""")

pdf.sub_title("GET /api/auth/kyc/status  [Auth Required]  --  Get KYC submission status")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": [
    {
      "documentId": "a1b2c3d4-...",
      "docType": "Passport",
      "status": "Approved",
      "submittedAt": "2026-03-31T10:05:00Z"
    }
  ]
}""")

# ══════════════════════════════════════════════════════════════════════════════
# 2. WALLET SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("2. Wallet Service  --  /api/wallet  [Auth Required]")
pdf.body("Manages wallet balance, top-ups, peer-to-peer transfers, and transaction history.")

pdf.sub_title("GET /api/wallet/balance  --  Get wallet balance")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "walletId": "b2c3d4e5-...",
    "balance": 2500.00,
    "currency": "INR",
    "isLocked": false,
    "kycVerified": true
  }
}""")

pdf.sub_title("POST /api/wallet/topup  --  Top up wallet balance")
pdf.code_block("Request Body:", """{
  "amount": 500.00,
  "provider": "Razorpay",
  "idempotencyKey": "topup-abc-123"
}""")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "transactionId": "c3d4e5f6-...",
    "amount": 500.00,
    "newBalance": 3000.00,
    "status": "Completed"
  }
}""")

pdf.sub_title("POST /api/wallet/transfer  --  Transfer to another user")
pdf.code_block("Request Body:", """{
  "toUserId": "d4e5f6a7-...",
  "amount": 200.00,
  "idempotencyKey": "transfer-xyz-456",
  "description": "Splitting dinner bill"
}""")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "transactionId": "e5f6a7b8-...",
    "amount": 200.00,
    "newBalance": 2800.00,
    "status": "Completed"
  }
}""")

pdf.sub_title("GET /api/wallet/transactions  --  Transaction history")
pdf.note("Query params: page=1, size=20, from=2026-03-01, to=2026-03-31")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "items": [
      {
        "id": "f6a7b8c9-...",
        "type": "TopUp",
        "amount": 500.00,
        "referenceType": "TopUpRequest",
        "description": "Wallet top-up via Razorpay",
        "createdAt": "2026-03-31T10:10:00Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20
  }
}""")

# ══════════════════════════════════════════════════════════════════════════════
# 3. REWARDS SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("3. Rewards Service  --  /api/rewards  [Auth Required]")
pdf.body("Handles points accumulation, tier tracking, catalog browsing, and redemptions.")

pdf.sub_title("GET /api/rewards/account  --  Get rewards account")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "id": "a7b8c9d0-...",
    "pointsBalance": 1200,
    "tier": "Silver",
    "lifetimePoints": 3500
  }
}""")

pdf.sub_title("GET /api/rewards/transactions  --  Points transaction history")
pdf.note("Query params: page=1, size=20")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "items": [
      {
        "id": "b8c9d0e1-...",
        "type": "Earned",
        "pointsDelta": 50,
        "referenceType": "TopUp",
        "description": "Points for top-up of INR 500",
        "createdAt": "2026-03-31T10:10:00Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20
  }
}""")

pdf.sub_title("GET /api/rewards/catalog  --  Browse rewards catalog")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": [
    {
      "id": "c9d0e1f2-...",
      "name": "Amazon Gift Card INR 100",
      "description": "Redeemable on Amazon India",
      "pointsCost": 500,
      "category": "GiftCards",
      "isAvailable": true,
      "stockQuantity": 100
    }
  ]
}""")

pdf.sub_title("POST /api/rewards/redeem  --  Redeem points for catalog item")
pdf.code_block("Request Body:", '{ "catalogItemId": "c9d0e1f2-..." }')
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "redemptionId": "d0e1f2a3-...",
    "pointsSpent": 500,
    "remainingBalance": 700,
    "status": "Fulfilled",
    "fulfillmentCode": "AMZN-XXXX-YYYY"
  }
}""")

pdf.sub_title("GET /api/rewards/redemptions  --  Get redemption history")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": [
    {
      "id": "d0e1f2a3-...",
      "catalogItemName": "Amazon Gift Card INR 100",
      "pointsSpent": 500,
      "status": "Fulfilled",
      "fulfillmentCode": "AMZN-XXXX-YYYY",
      "createdAt": "2026-03-31T11:00:00Z"
    }
  ]
}""")

# ══════════════════════════════════════════════════════════════════════════════
# 4. NOTIFICATION SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("4. Notification Service  --  /api/notifications  [Auth Required]")
pdf.body("View your notification history and manage templates (Admin only).")

pdf.sub_title("GET /api/notifications  --  Get notification history")
pdf.note("Query params: page=1, size=20")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "items": [
      {
        "id": "a3b4c5d6-...",
        "channel": "Email",
        "type": "TopUpSuccess",
        "subject": "Your wallet has been topped up",
        "status": "Sent",
        "createdAt": "2026-03-31T10:10:00Z",
        "sentAt": "2026-03-31T10:10:05Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20
  }
}""")

pdf.sub_title("GET /api/notifications/templates  [Admin]  --  Get all templates")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": [
    {
      "id": "b4c5d6e7-...",
      "type": "TopUpSuccess",
      "channel": "Email",
      "subject": "Your wallet has been topped up",
      "bodyTemplate": "Hi {{name}}, your wallet was topped up by {{amount}}.",
      "isActive": true
    }
  ]
}""")

pdf.sub_title("PUT /api/notifications/templates/{id}  [Admin]  --  Update template")
pdf.code_block("Request Body:", """{
  "subject": "Wallet Top-Up Successful",
  "bodyTemplate": "Hi {{name}}, INR {{amount}} added to your wallet.",
  "isActive": true
}""")
pdf.note("Response 200: Returns updated NotificationTemplateDto.")
pdf.ln(2)

# ══════════════════════════════════════════════════════════════════════════════
# 5. SUPPORT TICKET SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("5. Support Ticket Service  --  /api/support  [Auth Required]")
pdf.body("Users can create tickets and reply; Admins can triage, reply, and update status.")

pdf.sub_title("POST /api/support/tickets  --  Create a support ticket")
pdf.note("priority: Low | Medium | High | Urgent    category: Payment | Account | KYC | Rewards | Other")
pdf.code_block("Request Body:", """{
  "subject": "Transfer not reflecting",
  "description": "I transferred INR 200 but recipient hasn't received it.",
  "priority": "High",
  "category": "Payment"
}""")
pdf.code_block("Response 201:", """{
  "success": true,
  "data": {
    "id": "e1f2a3b4-...",
    "ticketNumber": "TKT-0001",
    "subject": "Transfer not reflecting",
    "status": "Open",
    "priority": "High",
    "category": "Payment",
    "description": "I transferred INR 200 but recipient hasn't received it.",
    "replyCount": 0,
    "replies": [],
    "createdAt": "2026-03-31T11:05:00Z",
    "updatedAt": "2026-03-31T11:05:00Z"
  }
}""")

pdf.sub_title("GET /api/support/tickets  --  Get my tickets")
pdf.note("Query params: page=1, size=20, status=Open|InProgress|Resolved|Closed")
pdf.code_block("Response 200 (summary list):", """{
  "success": true,
  "data": {
    "items": [
      {
        "id": "e1f2a3b4-...",
        "ticketNumber": "TKT-0001",
        "subject": "Transfer not reflecting",
        "status": "Open",
        "priority": "High",
        "category": "Payment",
        "replyCount": 1,
        "createdAt": "2026-03-31T11:05:00Z",
        "updatedAt": "2026-03-31T11:10:00Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20
  }
}""")

pdf.sub_title("POST /api/support/tickets/{ticketId}/replies  --  Add reply")
pdf.code_block("Request Body:", '{ "message": "Any update on this?" }')
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "id": "f2a3b4c5-...",
    "authorId": "3fa85f64-...",
    "authorRole": "User",
    "message": "Any update on this?",
    "createdAt": "2026-03-31T11:15:00Z"
  }
}""")

pdf.sub_title("POST /api/support/admin/tickets/{ticketId}/replies  [Admin]  --  Admin reply")
pdf.code_block("Request Body:", '{ "message": "We have escalated this to the payments team." }')
pdf.note("Response 200: Same TicketReplyDto shape with authorRole: Admin.")
pdf.ln(2)

pdf.sub_title("PATCH /api/support/admin/tickets/{ticketId}/status  [Admin]  --  Update status")
pdf.note("status: Open | InProgress | Resolved | Closed")
pdf.code_block("Request Body:", """{
  "status": "Resolved",
  "internalNote": "Confirmed transfer completed. User notified."
}""")
pdf.note("Response 200: Returns updated TicketSummaryDto.")
pdf.ln(2)

# ══════════════════════════════════════════════════════════════════════════════
# 6. ADMIN SERVICE
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("6. Admin Service  --  /api/admin  [Admin Role Required]")
pdf.body("Dashboard statistics, KYC review workflow, and bonus campaign management.")

pdf.sub_title("GET /api/admin/dashboard  --  Dashboard statistics")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "pendingKYCCount": 12,
    "approvedKYCToday": 5,
    "rejectedKYCToday": 2,
    "activeCampaigns": 3,
    "totalCampaigns": 10,
    "adminActionsToday": 7
  }
}""")

pdf.sub_title("GET /api/admin/kyc/pending  --  List pending KYC reviews")
pdf.note("Query params: page=1, size=20")
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "items": [
      {
        "id": "b4c5d6e7-...",
        "documentId": "a1b2c3d4-...",
        "userId": "3fa85f64-...",
        "docType": "Passport",
        "fileUrl": "https://storage.example.com/docs/passport.pdf",
        "status": "Pending",
        "reviewNotes": null,
        "submittedAt": "2026-03-31T10:05:00Z",
        "reviewedAt": null
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20
  }
}""")

pdf.sub_title("POST /api/admin/kyc/{id}/approve  --  Approve KYC document")
pdf.code_block("Request Body:", '{ "notes": "Document verified. Clear photo, valid expiry." }')
pdf.code_block("Response 200:", """{
  "success": true,
  "data": {
    "id": "b4c5d6e7-...",
    "documentId": "a1b2c3d4-...",
    "userId": "3fa85f64-...",
    "docType": "Passport",
    "fileUrl": "https://storage.example.com/docs/passport.pdf",
    "status": "Approved",
    "reviewNotes": "Document verified. Clear photo, valid expiry.",
    "submittedAt": "2026-03-31T10:05:00Z",
    "reviewedAt": "2026-03-31T11:30:00Z"
  }
}""")

pdf.sub_title("POST /api/admin/kyc/{id}/reject  --  Reject KYC document")
pdf.code_block("Request Body:", '{ "reason": "Document is blurry and unreadable." }')
pdf.note("Response 200: Same KYCReviewDto shape with status: Rejected.")
pdf.ln(2)

pdf.sub_title("POST /api/admin/campaigns  --  Create bonus campaign")
pdf.note("triggerType: TopUp | Transfer | All")
pdf.code_block("Request Body:", """{
  "name": "Summer Bonus",
  "description": "Double points on all top-ups",
  "triggerType": "TopUp",
  "bonusMultiplier": 2.0,
  "minimumAmount": 500.00,
  "startsAt": "2026-04-01T00:00:00Z",
  "endsAt": "2026-04-30T23:59:59Z"
}""")
pdf.code_block("Response 201:", """{
  "success": true,
  "data": {
    "id": "c5d6e7f8-...",
    "name": "Summer Bonus",
    "description": "Double points on all top-ups",
    "triggerType": "TopUp",
    "bonusMultiplier": 2.0,
    "minimumAmount": 500.00,
    "startsAt": "2026-04-01T00:00:00Z",
    "endsAt": "2026-04-30T23:59:59Z",
    "isActive": true,
    "createdAt": "2026-03-31T11:00:00Z"
  }
}""")

pdf.sub_title("PUT /api/admin/campaigns/{id}  --  Update campaign")
pdf.code_block("Request Body:", """{
  "description": "Triple points on top-ups this summer",
  "bonusMultiplier": 3.0,
  "minimumAmount": 300.00,
  "startsAt": "2026-04-01T00:00:00Z",
  "endsAt": "2026-04-30T23:59:59Z",
  "isActive": true
}""")
pdf.note("Response 200: Returns updated CampaignDto.")
pdf.ln(2)

pdf.sub_title("DELETE /api/admin/campaigns/{id}  --  Delete campaign")
pdf.note("Response 200: { \"success\": true, \"data\": {} }")
pdf.ln(2)

# ══════════════════════════════════════════════════════════════════════════════
# Rate Limits & Ports
# ══════════════════════════════════════════════════════════════════════════════
pdf.section_title("Rate Limits  (via Gateway -- per minute per client)")
pdf.rate_table([
    ("/api/auth",           "30 req/min",  "No"),
    ("/api/wallet",         "20 req/min",  "Yes"),
    ("/api/rewards",        "20 req/min",  "Yes"),
    ("/api/notifications",  "20 req/min",  "Yes"),
    ("/api/support",        "20 req/min",  "Yes"),
    ("/api/admin",          "15 req/min",  "Yes (Admin role)"),
])
pdf.code_block("429 Too Many Requests:", '{ "message": "Too many requests. Please try again later." }')

pdf.section_title("Service Ports  (Direct -- bypassing Gateway)")
pdf.port_table([
    ("Gateway",              "5000"),
    ("AuthService",          "5001"),
    ("WalletService",        "5002"),
    ("RewardsService",       "5003"),
    ("NotificationService",  "5004"),
    ("AdminService",         "5005"),
    ("SupportTicketService", "5006"),
])

pdf.output(OUTPUT)
print(f"PDF written to: {OUTPUT}")
