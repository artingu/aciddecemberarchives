# Set the cloud run project to always serve the latest image
gcloud auth login
gcloud config set project seismic-ground-286510
gcloud run services update-traffic aciddecemberarchives --to-latest
# ange region europe-north1