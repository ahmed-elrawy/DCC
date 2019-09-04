import { Photo } from './Photo';
 export interface User {
id: number ;
username: string ;
knownAs: string ;
age: number ;
dateOfBirth: Date ;

gender: string ;
created: Date ;
lastActive: Date ;
photoUrl: string ;
city: string ;
country: string ;
interests: string ;
introducation: string ;
lookingFor: string ;
photos: Photo[] ;
likerCount: number;
specialization: string ;
typeOfUser: string ;
}

 