import streamlit as st
import json
import os
import requests
from pathlib import Path


downloads_path = str(Path.home() / "Downloads")

paintingList = st.file_uploader("Select the list of paintings", ".json")
if paintingList is not None:
    paintingJson = json.load(paintingList)
    
    st.metric("Number of paintings",len(paintingJson))
    downloadButton = st.button("Start download")

    downloadPath = st.text_input("Download folder: ",downloads_path )


    if downloadButton:
        progressBar = st.progress(0,"Downloading")
        progressPercent = 0
        if os.path.exists(downloadPath):
            paintingFolder = os.path.join(downloadPath,"paintings")
            if not os.path.exists(paintingFolder):
                os.mkdir(paintingFolder)
            
            for idx, painting in enumerate(paintingJson):
                progressBar.progress(idx/len(paintingJson),"Downloading " +  painting.get("title").get("en"))
                paintingFiles = painting.get("multimedia")[0].get("jpg")
                maxResolution = sorted([int(key) for key in paintingFiles.keys()])[-1]
                paintingPath = paintingFiles[str(maxResolution)]
                fullPath = "https://www.kansallisgalleria.fi" + paintingPath
                st.write(fullPath)
                fileName = paintingPath.replace("/","-").removeprefix("-")
                filePath = os.path.join(paintingFolder,fileName)
                if not os.path.exists(filePath):
                    img_data = requests.get(fullPath).content
                    st.write(filePath)
                    with open(filePath,"wb") as handler:
                        handler.write(img_data)

            progressBar.empty()




    