gcloud auth login
gcloud config set project seismic-ground-286510
gcloud config set run/region europe-north1
gcloud run services update-traffic aciddecemberarchives --to-latest

