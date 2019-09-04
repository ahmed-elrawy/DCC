export interface Message {
    id: number;
    senderId: string;
    recipientId: string ;
    senderKnownAs: string;
    senderPhotoUrl: string;
    recipientKnownAs: string;
    recipientPhotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
  }
