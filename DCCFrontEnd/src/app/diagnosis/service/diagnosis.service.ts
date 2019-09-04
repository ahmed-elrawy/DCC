import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DiagnosisService {

constructor(private http: HttpClient) { }

getAllBodyAreas(){
  return this.http.get('/api/Diagnosis/getBodyAreas');
}
getAllSymptom(id){
  return this.http.get(`/api/Diagnosis/GetSympotByBodyAreasId?BodyId=${id}`);
}
getDrugBySymptomId(id){
  return this.http.get(`/api/Diagnosis/GetDrugBySyptomId?symptomId=${id}`);
}
getDrugById(id){
  return this.http.get(`/api/Diagnosis/GetDrugById?drugId=${id}`);
}
createRequest(data) {
  return this.http.post(`/api/Diagnosis/CreateRequest`, data);
}
}
