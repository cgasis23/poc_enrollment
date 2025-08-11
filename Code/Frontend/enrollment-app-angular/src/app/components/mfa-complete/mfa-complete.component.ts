import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

interface MFAResult {
  mfaVerified: boolean;
  mfaMethod: string | null;
}

@Component({
  selector: 'app-mfa-complete',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mfa-complete.component.html',
  styleUrl: './mfa-complete.component.css'
})
export class MFACompleteComponent {
  @Input() mfaResult!: MFAResult;

  goToDashboard() {
    window.location.href = '/dashboard';
  }

  goToLogin() {
    window.location.href = '/login';
  }
}
