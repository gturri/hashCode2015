rm -rf tempForArchive
cp -r finale tempForArchive
rm -rf tempForArchive/bin

tar -czf $(date +%H_%M_%S)_code.tgz tempForArchive
